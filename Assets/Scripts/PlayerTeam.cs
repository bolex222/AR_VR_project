using Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerTeam : MonoBehaviourPunCallbacks
{
    [SerializeField] public AllGenericTypes.Team team;
    [SerializeField] private TeleportationProvider teleprovider;

    private void Start()
    {
        var teleArea = GameObject.Find("floor").GetComponent<TeleportationArea>();
        
        if (photonView.IsMine && teleArea is not null)
        {
            teleArea.teleportationProvider = teleprovider;
        }
    }
}
