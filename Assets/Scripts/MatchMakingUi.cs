using System;
using Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class MatchMakingUi : MonoBehaviour
{
    [SerializeField] private Canvas vrTeamSelectionCanvas;
    [SerializeField] private Canvas vrStartGameCanvas;

    [SerializeField] private UIDocument pcTeamSelection;
    [SerializeField] private UIDocument pcStartGame;

    [SerializeField] private MatchMakingNetworkManager matchMakingNetworkManager;
    [SerializeField] private GameObject handRayInteractor;

    private bool _isUserVr;

    private XRRayInteractor vrRay;
    private LineRenderer rayRenderer;
    private XRInteractorLineVisual rayInteractor;

    private void Start()
    {
        _isUserVr = UnityEngine.XR.XRSettings.isDeviceActive;
        DisplayTeamSelection();

        if (_isUserVr)
        {
            Debug.Log(matchMakingNetworkManager);

           vrRay = handRayInteractor.GetComponent<XRRayInteractor>();
           rayRenderer = handRayInteractor.GetComponent<LineRenderer>();
           rayInteractor = handRayInteractor.GetComponent<XRInteractorLineVisual>();
        }
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

    public void activeRay()
    {
        vrRay.enabled = true;
        rayRenderer.enabled = true;
        rayInteractor.enabled = true;
    }
}