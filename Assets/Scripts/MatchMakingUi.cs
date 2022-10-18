using Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class MatchMakingUi : MonoBehaviour
{
    [SerializeField] private Canvas vrTeamSelectionCanvas;
    [SerializeField] private Canvas vrStartGameCanvas;

    [SerializeField] private UIDocument pcTeamSelection;
    [SerializeField] private UIDocument pcStartGame;

    [SerializeField] private MatchMakingNetworkManager matchMakingNetworkManager;

    private bool _isUserVr;

    private void Start()
    {
        _isUserVr = UnityEngine.XR.XRSettings.isDeviceActive;
        DisplayTeamSelection();
    }

    private void DisplayTeamSelection()
    {
        if (_isUserVr)
        {
            vrTeamSelectionCanvas.gameObject.SetActive(true);
            return;
        }

        pcTeamSelection.gameObject.SetActive(true);
    }

    public void OnSelectTeam(AllGenericTypes.Team team)
    {
        pcTeamSelection.gameObject.SetActive(false);
        vrTeamSelectionCanvas.gameObject.SetActive(false);
        
        matchMakingNetworkManager.JoinTeam(team);

        if (!PhotonNetwork.IsMasterClient) return;

        if (_isUserVr)
        {
            vrStartGameCanvas.gameObject.SetActive(true);
        }
        else
        {
            pcStartGame.gameObject.SetActive(true);
        }
    }

    public void OnStartGame()
    {
        matchMakingNetworkManager.StartGame();
    }
}