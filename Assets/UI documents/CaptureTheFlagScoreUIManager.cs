using System;
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
            //Debug.Log(uiDocument);
            _scoreUIElement = uiDocument.rootVisualElement;
            Debug.Log(_scoreUIElement.name);

            _teamAScoreUI = _scoreUIElement.Q<VisualElement>("team-a-score");
            _teamBScoreUI = _scoreUIElement.Q<VisualElement>("team-b-score");
            _fullScore = _scoreUIElement.Q<Label>("full-score");
            
            Debug.Log(_teamAScoreUI);
            Debug.Log(_teamBScoreUI);   
        }
        
        private void OnEnable()
        {
            SetUp();   
        }
        
        public void UpdateScore(int scoreTeamA, int scoreTeamB, int totalCaptureZone)
        {
            float a = scoreTeamA;
            float b = scoreTeamB;
            float T = (float)totalCaptureZone;
            float percentA = a / T * 100;
            float percentB = b / T * 100;
            Debug.Log(a / T * 100);
            Debug.Log(b / T * 100);
            _teamAScoreUI.style.width = new StyleLength(Length.Percent(percentA));
            _teamBScoreUI.style.width = new StyleLength(Length.Percent(percentB));
            _fullScore.text = $"{scoreTeamA} | {scoreTeamB}";

        }
    }
}
