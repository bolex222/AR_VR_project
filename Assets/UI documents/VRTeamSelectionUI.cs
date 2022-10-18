using Interfaces;
using UnityEngine;

namespace UI_documents
{
    public class VRTeamSelectionUI : MonoBehaviour
    {
        [SerializeField] private MatchMakingUi matchMakingUi;
        
        public void OnSelectTeamA()
        {
            matchMakingUi.OnSelectTeam(AllGenericTypes.Team.TeamA);
        }
        
        public void OnSelectTeamB()
        {
            matchMakingUi.OnSelectTeam(AllGenericTypes.Team.TeamB);
        }
    }
}
