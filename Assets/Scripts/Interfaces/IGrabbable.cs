using UnityEngine;

namespace Interfaces
{
    public interface IGrabbable
    {
        public void Grab(RaycastHit hit, GameObject gameObject);
        public void Drop(RaycastHit hit, GameObject gameObject);
    }   
}
