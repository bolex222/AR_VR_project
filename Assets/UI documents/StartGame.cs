using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI_documents
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private MatchMakingNetworkManager matchmakingNetworkManager;

        private Button _startGameButton;

        private void OnEnable()
        {
            var root = uiDocument.rootVisualElement;
            _startGameButton = root.Q<Button>("start-game");
            _startGameButton.clicked += Onclick;
        }

        private void Onclick()
        {
            matchmakingNetworkManager.StartGame();
            gameObject.SetActive(false);
        }
    }
}