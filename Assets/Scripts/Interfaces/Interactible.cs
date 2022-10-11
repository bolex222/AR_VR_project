using PlayerScripts;
using TPS_Player;
using UnityEngine;

namespace Interfaces
{
    public class Interactible : MonoBehaviour, IInteractible
    {
        public void Interact(RaycastHit raycastHit, Player player) {}
    }
}
