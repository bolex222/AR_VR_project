using Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum GameModeOptions
{
    DeathMatch,
    CaptureTheFlag
}

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public GameModeOptions gameMode;

    private IGameBehaviour _game;

    public GameObject teamAPlayerPrefabPC;
    public GameObject teamBPlayerPrefabPC;

    public GameObject teamAPlayerPrefabVR;
    public GameObject teamBPlayerPrefabVR;

    [SerializeField] private GameObject pioupiouPrefab;

    private void Start()
    {
        Instance = this;
        switch (gameMode)
        {
            case GameModeOptions.DeathMatch:
                break;
            case GameModeOptions.CaptureTheFlag:
                _game = gameObject.GetComponent<CaptureTheFlag>();
                break;
        }

        SetupGame();
    }

    private void SetupGame()
    {
        Spawn();
        _game.SetUpGame();
        _game.GameStart();
    }

    
    public void Spawn()
    {
        
        // INSTANTIATE THE RIGHT PREFAB DEPENDING ON THE USER TEAM AND DEVICE
        
        Debug.Log( "current player in A? : "+ MatchMakingNetworkManager.playersTeamA.Contains(PhotonNetwork.LocalPlayer));
        Debug.Log( "current player in B? : "+ MatchMakingNetworkManager.playersTeamB.Contains(PhotonNetwork.LocalPlayer));
        
        AllGenericTypes.Team team = MatchMakingNetworkManager.playersTeamA.Contains(PhotonNetwork.LocalPlayer)
            ? AllGenericTypes.Team.TeamA
            : AllGenericTypes.Team.TeamB;

        GameObject htcPrefabForRightTeam =
            team == AllGenericTypes.Team.TeamA ? teamAPlayerPrefabVR : teamBPlayerPrefabVR;

        GameObject pcPrefabForRightTeam =
            team == AllGenericTypes.Team.TeamA ? teamAPlayerPrefabPC : teamBPlayerPrefabPC;

        GameObject playerPrefab =
            UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(pcPrefabForRightTeam, htcPrefabForRightTeam);

        Transform spawn =
            team == AllGenericTypes.Team.TeamA
                ? SpawnerManager.instance.GetTeamSpawn(0)
                : SpawnerManager.instance.GetTeamSpawn(1);


        UserDeviceType userDeviceType = UserDeviceManager.GetDeviceUsed();

        Vector3 initialPos = userDeviceType == UserDeviceType.HTC
            ? new Vector3(0f, 1f, 0f)
            : new Vector3(0f, 5f, 0f);
        
        // INSTANTIATE THE PLAYER
        GameObject player = PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, spawn.position, spawn.rotation);

        if (userDeviceType == UserDeviceType.HTC)
        {
            GameObject pioupiou =
                PhotonNetwork.Instantiate("Prefabs/" + pioupiouPrefab.name, initialPos, Quaternion.identity);

            SocketInteractor pioupiouSocketInteractor = pioupiou.GetComponentInChildren<SocketInteractor>();
            Pioupiou pioupiouScript = pioupiou.GetComponent<Pioupiou>();
            XRSocketInteractor playerSocket = player.GetComponentInChildren<XRSocketInteractor>();
            pioupiouScript.playerTeam = team;
            
            if (pioupiouSocketInteractor != null && playerSocket != null)
            {
                pioupiouSocketInteractor.xrSocketInteractor = playerSocket;
                pioupiouSocketInteractor.GunTp();
            }
        }
    }
}