using UnityEngine;
using UnityEngine.UIElements;

namespace UI_documents
{
    public class StartGameUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private MatchMakingUi matchMakingUi;

        private Button _startGameButton;

        private void OnEnable()
        {
            var root = uiDocument.rootVisualElement;
            _startGameButton = root.Q<Button>("start-game");
            _startGameButton.clicked += Onclick;
        }

        private void Onclick()
        {
            matchMakingUi.OnStartGame();
            gameObject.SetActive(false);
        }
    }
}