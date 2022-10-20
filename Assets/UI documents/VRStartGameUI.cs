using System;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

namespace UI_documents
{
    public class VRStartGameUI : MonoBehaviour
    {
        [SerializeField] private ButtonScript startGameButton;
        [SerializeField] private TextMeshProUGUI warningText;
        [SerializeField] private MatchMakingNetworkManager matchMakingNetworkManager;
        [SerializeField] private TextMeshProUGUI playerQuantity;

        private GameObject buttonElem;

        private void Awake()
        {
            buttonElem = startGameButton.gameObject;
        }


        public void Update()
        {
            
            string plural = PhotonNetwork.CurrentRoom.PlayerCount > 1 ? "s" : "";
            playerQuantity.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} player{plural} connected";
            if (matchMakingNetworkManager.CanStartGame())
            {
                buttonElem.SetActive(true);
                warningText.enabled = false;
            }
            else
            {
                buttonElem.SetActive(false);
                warningText.enabled = true;
            }
        }
    }
}