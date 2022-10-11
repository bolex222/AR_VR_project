using PlayerScripts;
using TPS_Player;
using UnityEngine;

namespace Interfaces
{
    public interface IInteractible
    {
        public void Interact(RaycastHit raycastHit, Player gameObject);
    }
}
