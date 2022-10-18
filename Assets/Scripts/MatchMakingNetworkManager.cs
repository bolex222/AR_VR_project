using System;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Interfaces;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Cursor = UnityEngine.UIElements.Cursor;

public class MatchMakingNetworkManager : MonoBehaviourPunCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
{
    public static MatchMakingNetworkManager Instance;

    public static List<Player> playersTeamA = new List<Player>();
    public static List<Player> playersTeamB = new List<Player>();
    [SerializeField] private GameObject genericPCPlayerPrefab;
    [SerializeField] private GameObject genericVRPlayerPrefab;
    [SerializeField] private UIDocument startButtonUi;


    /*[Tooltip("The prefab to use for representing the user on a PC. Must be in Resources folder")]
    public GameObject playerPrefabPC;

    [Tooltip("The prefab to use for representing the user in VR. Must be in Resources folder")]
    public GameObject playerPrefabVR;*/

    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. 
    /// </summary>
    public override void OnLeftRoom()
    {
        // TODO: load the Lobby Scene
        SceneManager.LoadScene("Lobby");
    }

    /// <summary>
    /// Called when Other Player enters the room and Only other players
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        // TODO: 
    }

    /// <summary>
    /// Called when Other Player leaves the room and Only other players
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        // TODO: 
    }

    #endregion

    // Start is called before the first frame update

    #region Public Methods

    /// <summary>
    /// Our own function to implement for leaving the Room
    /// </summary>
    public void LeaveRoom()
    {
        // TODO: 
        PhotonNetwork.LeaveRoom();
    }

    private void updatePlayerNumberUI()
    {
        // TODO: Update the playerNumberUI
    }

    void Start()
    {

        Instance = this;

        GameObject playerPrefab =
            UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(genericPCPlayerPrefab, genericVRPlayerPrefab);
        if (playerPrefab == null)
        {
            Debug.LogErrorFormat(
                "<Color=Red><a>Missing</a></Color> playerPrefab Reference for device {0}. Please set it up in GameObject 'NetworkManager'",
                UserDeviceManager.GetDeviceUsed());
        }
        else
        {
            // TODO: Instantiate the prefab representing my own avatar only if it is UserMe
            if (UserManager.UserMeInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                Vector3 initialPos = UserDeviceManager.GetDeviceUsed() == UserDeviceType.HTC
                    ? new Vector3(0f, 1f, 0f)
                    : new Vector3(0f, 5f, 0f);
                GameObject gm = PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, initialPos, Quaternion.identity, 0);
                // Debug.Log($"Player id by photon : {PhotonNetwork}");
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            // Code to leave the room by pressing CTRL + the Leave button
            if (Input.GetButtonUp("Leave"))
            {
                Debug.Log("Leave event");
                LeaveRoom();
            }
        }
    }

    public void JoinTeam(AllGenericTypes.Team team)
    {
        Debug.Log(playersTeamA.Contains(PhotonNetwork.LocalPlayer));
        Debug.Log(playersTeamB.Contains(PhotonNetwork.LocalPlayer));
        if (team == AllGenericTypes.Team.TeamA && !playersTeamA.Contains(PhotonNetwork.LocalPlayer))
        {
            Debug.Log("Player" + PhotonNetwork.LocalPlayer.NickName + "added in A");
            playersTeamA.Add(PhotonNetwork.LocalPlayer);
        }
        else if (team == AllGenericTypes.Team.TeamB && !playersTeamB.Contains(PhotonNetwork.LocalPlayer))
        {
            Debug.Log("Player" + PhotonNetwork.LocalPlayer.NickName + "added in B");
            playersTeamB.Add(PhotonNetwork.LocalPlayer);
        }

        Debug.Log($"Team A {playersTeamA.Count} | Team B {playersTeamB.Count}");
    }

    public void StartGame()
    {
        Debug.Log("game start");
        //TODO
        SceneManager.LoadScene("map");
    }

    #endregion
}