using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Interfaces;
using UnityEngine.UIElements;

public class MatchMakingNetworkManager : MonoBehaviourPunCallbacks
{
    public static MatchMakingNetworkManager Instance;

    public static List<Player> playersTeamA = new List<Player>();
    public static List<Player> playersTeamB = new List<Player>();
    [SerializeField] private GameObject genericPCPlayerPrefab;
    [SerializeField] private GameObject genericVRPlayerPrefab;
    [SerializeField] private UIDocument startButtonUi;
    
    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. 
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    #endregion

    // Start is called before the first frame update

    #region Public Methods

    /// <summary>
    /// Our own function to implement for leaving the Room
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
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
            if (UserManager.UserMeInstance == null)
            {
                //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                Vector3 initialPos = UserDeviceManager.GetDeviceUsed() == UserDeviceType.HTC
                    ? new Vector3(0f, 1f, 0f)
                    : new Vector3(0f, 5f, 0f);
                PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, initialPos, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
    //     {
    //         // Code to leave the room by pressing CTRL + the Leave button
    //         if (Input.GetButtonUp("Leave"))
    //         {
    //             LeaveRoom();
    //         }
    //     }
    // }

    public void OnAddPlayerToTeam(AllGenericTypes.Team team)
    {
        // photonView.RPC("JoinTeam", RpcTarget.AllViaServer, team);
        JoinTeam(team);
    }

    // [PunRPC]
    private void JoinTeam(AllGenericTypes.Team team)
    {
        if (team == AllGenericTypes.Team.TeamA && !playersTeamA.Contains(PhotonNetwork.LocalPlayer))
        {
            playersTeamA.Add(PhotonNetwork.LocalPlayer);
        }
        else if (team == AllGenericTypes.Team.TeamB && !playersTeamB.Contains(PhotonNetwork.LocalPlayer))
        {
            playersTeamB.Add(PhotonNetwork.LocalPlayer);
        }
        Debug.Log($"team A: {playersTeamA.Count} | team B: {playersTeamB.Count}");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("map");
    }

    #endregion
}