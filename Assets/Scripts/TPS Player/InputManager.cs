using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TPS_Player
{
    public class InputManager : MonoBehaviour
    {
    
        public static InputManager Instance;

        private PlayerInput _mPlayerInput;
        private InputAction _mMoveAction;
        private InputAction _mLookAction;
        private InputAction _mJumpAction;
        private InputAction _mSpaceAction;
        private InputAction _mInteractAction;
        private InputAction _mShiftAction;
        public IControllable Controllable;

    
    
        private void Awake()
        {
            Instance = this;
            _mPlayerInput = GetComponent<PlayerInput>();
            _mMoveAction = _mPlayerInput.actions["move"];
            _mLookAction = _mPlayerInput.actions["look"];
            _mJumpAction = _mPlayerInput.actions["jump"];
            _mShiftAction = _mPlayerInput.actions["sprint"];
            _mInteractAction = _mPlayerInput.actions["interact"];
            _mInteractAction.performed += ctx => CallInteraction();

        }



        public Vector2 GetPlayerInput()
        {
            return _mMoveAction.ReadValue<Vector2>();
        }

        public bool IsInteracting()
        {
            return _mInteractAction.ReadValue<float>() > 0;
        }

        public Vector2 GetMousePosition()
        {
            return _mLookAction.ReadValue<Vector2>();
        }

        public float GetSpaceButtonValue()
        {
            return _mJumpAction.ReadValue<float>();
        }

        public float GetShiftValue()
        {
            return _mShiftAction.ReadValue<float>();
        }

        private void CallInteraction()
        {
            Controllable.PressInteract(); 
        }
    
    
    }
}

