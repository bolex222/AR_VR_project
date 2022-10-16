using System;
using Interfaces;
using PlayerScripts;
using UnityEngine;

namespace TPS_Player
{
    public class Player : MonoBehaviour, IControllable
    {
        public bool isPlayerActive = true;

        private InputManager _mInputManager;
        
        [SerializeField] float mouseSensitivity = 3f;
        [SerializeField] float movmentSpeed = 2.5f;
        [SerializeField] float jumpSpeed = 5f;
        [SerializeField] float mass = 1f;
        [SerializeField] float accesleration = 10f;
        [SerializeField] Camera playerCamera;


        internal float MovmentSpeedMultiplier;

        public event Action OnBeforeMove;

        public Collider playerCollider;

        private CharacterController _mController;
        private Vector3 _mVelocity;
        private Vector2 _mLook;
        private bool _mIsHolding;

        RayCasting _mRayCasting;

        void Start()
        {
            //TODO check cursor du cul
            // Cursor.lockState = CursorLockMode.Locked;
        }


        // Start is called before the first frame update
        void Awake()
        {
            _mController = GetComponent<CharacterController>();
            _mRayCasting = GetComponent<RayCasting>();
            _mInputManager = InputManager.Instance;
            playerCollider = GetComponent<Collider>();
            EnablePlayer();
        }


        Vector3 GetMovmentInput()
        {
            Vector2 moveInput = _mInputManager.GetPlayerInput();

            Vector3 input = new Vector3();

            var transform1 = transform;
            input += transform1.forward * moveInput.y;
            input += transform1.right * moveInput.x;
            input = Vector3.ClampMagnitude(input, 1f);
            input *= movmentSpeed * MovmentSpeedMultiplier;
            return input;
        }

        void UpdateMovment()
        {
            MovmentSpeedMultiplier = 1f;
            OnBeforeMove?.Invoke();

            Vector3 input = GetMovmentInput();

            float factor = accesleration * Time.deltaTime;
            _mVelocity.x = Mathf.Lerp(_mVelocity.x, input.x, factor);
            _mVelocity.z = Mathf.Lerp(_mVelocity.z, input.z, factor);

            float jumpInput = _mInputManager.GetSpaceButtonValue();
            if (jumpInput > 0 && _mController.isGrounded)
            {
                _mVelocity.y += jumpSpeed;
            }

            _mController.Move((input * movmentSpeed + _mVelocity) * Time.deltaTime);
        }

        void UpdateLook()
        {
            Vector2 lookInput = _mInputManager.GetMousePosition();

            _mLook.x += lookInput.x * mouseSensitivity;
            // _mLook.y += lookInput.y * mouseSensitivity;

            // _mLook.y = Mathf.Clamp(_mLook.y, -89f, 89f);

            // playerCamera.transform.localRotation = Quaternion.Euler(-_mLook.y, 0, 0);
            transform.localRotation = Quaternion.Euler(0, _mLook.x, 0);
        }

        void UpdateGravity()
        {
            Vector3 gravity = Physics.gravity * mass * Time.deltaTime;
            _mVelocity.y = _mController.isGrounded ? -1f : _mVelocity.y + gravity.y;
        }

        // ReSharper disable Unity.PerformanceAnalysis

        public void PressInteract()
        {
            Interact();
        }
        
        void Update()
        {
            if (!isPlayerActive) return;
            UpdateMovment();
            UpdateLook();
            UpdateGravity();
        }

        void Interact()
        {
            RaycastHit hit = _mRayCasting.GetHitFromRayCast();

            if (_mIsHolding)
            {
                IGrabbable grabbedElement = GetComponentInChildren<IGrabbable>();
                if (grabbedElement is not null)
                {
                    grabbedElement.Drop(hit, gameObject);
                }

                _mIsHolding = false;
                return;
            }

            IInteractible interactibleObject = hit.collider.gameObject.GetComponent<IInteractible>();
            if (interactibleObject is not null)
            {
                interactibleObject.Interact(hit, this);
                return;
            }

            IGrabbable grabbableObject = hit.collider.gameObject.GetComponent<IGrabbable>();
            if (grabbableObject is not null)
            {
                grabbableObject.Grab(hit, gameObject);
                _mIsHolding = true;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            if (body == null || body.isKinematic)
            {
                return;
            }

            body.AddForceAtPosition(_mVelocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        public void DisablePlayer()
        {
            playerCamera.enabled = isPlayerActive = false;
            playerCollider.enabled = false;
        }
    
        public void EnablePlayer()
        {
            _mInputManager.Controllable = this;
            playerCollider.enabled = true;
            playerCamera.enabled = isPlayerActive = true;
            
        }
    
    }
}