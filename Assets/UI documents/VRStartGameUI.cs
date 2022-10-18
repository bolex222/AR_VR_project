using UnityEngine;

namespace UI_documents
{
    public class VRStartGameUI : MonoBehaviour
    {
        [SerializeField] private MatchMakingUi matchMakingUi;

        public void OnStartGame()
        {
            matchMakingUi.OnStartGame();
        }
    }
}