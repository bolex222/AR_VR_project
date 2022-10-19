using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI_documents
{
    public class VRStartGameUI : MonoBehaviour
    {
        [SerializeField] private MatchMakingUi matchMakingUi;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private TextMeshPro warningText;
        [SerializeField] private MatchMakingNetworkManager matchMakingNetworkManager;
        [SerializeField] private TextMeshPro playerQuantity;

        public void OnStartGame()
        {
            matchMakingUi.OnStartGame();
        }

        public void Update()
        {
            
            string plural = PhotonNetwork.CurrentRoom.PlayerCount > 1 ? "s" : "";
            playerQuantity.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} player{plural} connected";
            if (matchMakingNetworkManager.CanStartGame())
            {
                _startGameButton.style.display = DisplayStyle.Flex;
                warningText.enabled = false;
            }
            else
            {
                _startGameButton.style.display = DisplayStyle.None;
                warningText.enabled = true;
            }
        }
    }
}