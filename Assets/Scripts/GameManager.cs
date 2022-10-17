using Interfaces;
using Photon.Pun;
using UnityEngine;

public enum GameModeOptions {DeathMatch, CaptureTheFlag}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameModeOptions gameMode;

    private IGameBehaviour _game;

    public GameObject teamAPlayerPrefabPC;
    public GameObject teamBPlayerPrefabPC;

    public GameObject teamAPlayerPrefabVR;
    public GameObject teamBPlayerPrefabVR;

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
        Spawn();
        _game.SetUpGame();
        _game.GameStart();
    }

    public void Spawn()
    {
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
            team == AllGenericTypes.Team.TeamA ? SpawnerManager.instance.GetTeamSpawn(0) : SpawnerManager.instance.GetTeamSpawn(1);


        Vector3 initialPos = UserDeviceManager.GetDeviceUsed() == UserDeviceType.HTC
            ? new Vector3(0f, 1f, 0f)
            : new Vector3(0f, 5f, 0f);
        PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, spawn.position, spawn.rotation);
    }


}