using UnityEngine;
using UnityEngine.UIElements;

namespace UI_documents
{
    public class CaptureTheFlagScoreUIManager : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private VisualElement _teamAScoreUI;
        private VisualElement _teamBScoreUI;
        private Label _fullScore;

        private VisualElement _scoreUIElement;

        private void SetUp()
        {
            _scoreUIElement = uiDocument.rootVisualElement;
            _teamAScoreUI = _scoreUIElement.Q<VisualElement>("team-a-score");
            _teamBScoreUI = _scoreUIElement.Q<VisualElement>("team-b-score");
            _fullScore = _scoreUIElement.Q<Label>("full-score");
        }

        private void OnEnable()
        {
            SetUp();
        }

        public void UpdateScore(int scoreTeamA, int scoreTeamB, int totalCaptureZone)
        {
            float a = scoreTeamA;
            float b = scoreTeamB;
            float T = totalCaptureZone;

            _teamAScoreUI.style.width = new StyleLength(Length.Percent(a / T * 100));
            _teamBScoreUI.style.width = new StyleLength(Length.Percent(b / T * 100));
            _fullScore.text = $"{scoreTeamA} | {scoreTeamB}";
        }
    }
}