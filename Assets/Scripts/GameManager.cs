using Interfaces;
using Photon.Pun;
using UnityEngine;

public enum GameModeOptions {DeathMatch, CaptureTheFlag}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameModeOptions gameMode;

    private IGameBehaviour _game;

    public GameObject TeamAPlayerPrefabPC;
    public GameObject TeamBPlayerPrefabPC;

    public GameObject TeamAPlayerPrefabVR;
    public GameObject TeamBPlayerPrefabVR;

    private void Start()
    {
        Instance = this;
        switch (gameMode)
        {
            case GameModeOptions.DeathMatch:
                Debug.Log("game is a deathMatch");
                break;
            case GameModeOptions.CaptureTheFlag:
                _game = gameObject.GetComponent<CaptureTheFlag>();
                break;

        }

        SetupGame();
    }

    private void SetupGame()
    {
        AllGenericTypes.Team team = MatchMakingNetworkManager.playersTeamA.Contains(PhotonNetwork.LocalPlayer)
            ? AllGenericTypes.Team.TeamA
            : AllGenericTypes.Team.TeamB;
        
        Debug.Log(team);

        GameObject htcPrefabForRightTeam =
            team == AllGenericTypes.Team.TeamA ? TeamAPlayerPrefabVR : TeamBPlayerPrefabVR;

        GameObject pcPrefabForRightTeam =
            team == AllGenericTypes.Team.TeamA ? TeamAPlayerPrefabPC : TeamBPlayerPrefabPC;

        GameObject playerPrefab =
            UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(pcPrefabForRightTeam, htcPrefabForRightTeam);


        Vector3 initialPos = UserDeviceManager.GetDeviceUsed() == UserDeviceType.HTC
            ? new Vector3(0f, 1f, 0f)
            : new Vector3(0f, 5f, 0f);
        GameObject gm = PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, initialPos, Quaternion.identity);

        _game.SetUpGame();
        _game.GameStart();
    }
}

//     private void SetupGameSample()
//     {
//         Debug.Log("ENTER START");
//
//         //instantiate the blue player if team is 0 and red if it is not
//         if (NetworkManager.playersTeamA.Contains(PhotonNetwork.LocalPlayer))
//         {
//             //get a spawn for the correct team
//             Transform spawn = SpawnerManager.instance.GetTeamSpawn(0);
//            
//
//             GameObject playerPrefabA = UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(TeamAPlayerPrefabPC, TeamAPlayerPrefabVR);
//
//
//             if (playerPrefabA == null)
//             {
//                 Debug.LogErrorFormat(
//                     "<Color=Red><a>Missing</a></Color> playerPrefab Reference for device {0}. Please set it up in GameObject 'NetworkManager'",
//                     UserDeviceManager.GetDeviceUsed());
//             }
//             else
//             {
//                 // TODO: Instantiate the prefab representing my own avatar only if it is UserMe
//                 if (UserManager.UserMeInstance == null)
//                 {
//                     Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
//                     // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
//                     Vector3 initialPos = UserDeviceManager.GetDeviceUsed() == UserDeviceType.HTC
//                         ? new Vector3(0f, 1f, 0f)
//                         : new Vector3(0f, 5f, 0f);
//                     PhotonNetwork.Instantiate("Prefabs/" + playerPrefabA.name, spawn.position, spawn.rotation);
//                 }
//                 else
//                 {
//                     Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
//                 }
//             }
//         }
//         else if(NetworkManager.playersTeamB.Contains(PhotonNetwork.LocalPlayer))
//         {
//             //get a spawn for the correct team
//             Transform spawn = SpawnerManager.instance.GetTeamSpawn(1);
//
//
//             GameObject playerPrefabA = UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(TeamAPlayerPrefabPC, TeamAPlayerPrefabVR);
//
//             if (playerPrefabA == null)
//             {
//                 Debug.LogErrorFormat(
//                     "<Color=Red><a>Missing</a></Color> playerPrefab Reference for device {0}. Please set it up in GameObject 'NetworkManager'",
//                     UserDeviceManager.GetDeviceUsed());
//             }
//             else
//             {
//                 // TODO: Instantiate the prefab representing my own avatar only if it is UserMe
//                 if (UserManager.UserMeInstance == null)
//                 {
//                     Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
//                     // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
//                     Vector3 initialPos = UserDeviceManager.GetDeviceUsed() == UserDeviceType.HTC
//                         ? new Vector3(0f, 1f, 0f)
//                         : new Vector3(0f, 5f, 0f);
//                     PhotonNetwork.Instantiate("Prefabs/" + playerPrefabA.name, spawn.position, spawn.rotation);
//                 }
//                 else
//                 {
//                     Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
//                 }
//             }
//         }
//         
//     }
// }
