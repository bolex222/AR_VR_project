using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI_documents
{
    public class StartGameUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private MatchMakingUi matchMakingUi;
        [SerializeField] private MatchMakingNetworkManager matchMakingNetworkManager;

        private Button _startGameButton;
        private Label _warningLabel;
        private Label _playerCounter;

        private void OnEnable()
        {
            var root = uiDocument.rootVisualElement;
            _startGameButton = root.Q<Button>("start-game");
            _warningLabel = root.Q<Label>("not-all-player");
            _playerCounter = root.Q<Label>("player-counter");
            _startGameButton.clicked += Onclick;
        }

        private void Update()
        {
            string plural = PhotonNetwork.CurrentRoom.PlayerCount > 1 ? "s" : "";
            _playerCounter.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} player{plural} connected";
            if (matchMakingNetworkManager.CanStartGame())
            {
                _startGameButton.SetEnabled(true);
                _warningLabel.style.display = DisplayStyle.None;
            }
            else
            {
                _startGameButton.SetEnabled(false);
                _warningLabel.style.display = DisplayStyle.Flex;
            }
        }

        private void Onclick()
        {
            matchMakingUi.OnStartGame();
            gameObject.SetActive(false);
        }
    }
}