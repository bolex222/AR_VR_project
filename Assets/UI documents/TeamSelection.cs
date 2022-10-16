using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI_documents
{
   public class TeamSelection : MonoBehaviour
   {
      [SerializeField] private UIDocument uiDocument;
      [SerializeField] private MatchMakingNetworkManager matchmakingNetworkManager;

      private Button _teamAButton;
      private Button _teamBButton;

      private void OnEnable()
      {
         var root = uiDocument.rootVisualElement;

         _teamAButton = root.Q<Button>("team-a-button");
         _teamBButton = root.Q<Button>("team-b-button");


         _teamAButton.clicked += () => OnclickAnyButton(AllGenericTypes.Team.TeamA);
         _teamBButton.clicked += () => OnclickAnyButton(AllGenericTypes.Team.TeamB);
      }

      private void OnclickAnyButton(AllGenericTypes.Team team)
      {
         matchmakingNetworkManager.JoinTeam(team);
         gameObject.SetActive(false);
      }
   }
}
