using UnityEngine;

namespace VR_Player.scipts
{
    public class HandPhysicsBehaviour : MonoBehaviour
    {

        public Transform target;
        private Rigidbody _mRigidBody;
        private Collider[] _handColliders;

        private void Awake()
        {
            _mRigidBody = GetComponent<Rigidbody>();
            _handColliders = GetComponentsInChildren<Collider>();
        }

        public void DelayColliderEnable (float delay)
        {
            Invoke("EnableHandColliders", delay);
        }

        public void EnableHandColliders()
        {
            foreach (Collider handCollider in _handColliders)
            {
                handCollider.enabled = true;
            }
        }

        public void DisableHandColliders()
        {
            foreach (Collider handCollider in _handColliders)
            {
                handCollider.enabled = false;
            }
        }
        private void FixedUpdate()
        {
            _mRigidBody.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

            Quaternion rotationDiff = target.rotation * Quaternion.Inverse(transform.rotation);
            rotationDiff.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

            Vector3 rotationDiffInDegree = angleInDegree * rotationAxis;
            _mRigidBody.angularVelocity = (rotationDiffInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
        }
    }
}
