using UnityEngine;

namespace PlayerScripts
{
    public class RayCasting : MonoBehaviour
    {

        private RaycastHit m_Hit;
        [SerializeField] Camera rayCastCamera;

        private void FixedUpdate() {
            if(!rayCastCamera.enabled) return;
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            Physics.Raycast(rayCastCamera.transform.position, rayCastCamera.transform.forward, out m_Hit, Mathf.Infinity, layerMask);
        }

        public RaycastHit GetHitFromRayCast() {
            return m_Hit;
        }
    }
}
