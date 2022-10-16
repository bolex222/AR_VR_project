using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PlayerNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject TeamAPlayerPrefabPC;
    public GameObject TeamBPlayerPrefabPC;

    public GameObject TeamAPlayerPrefabVR;
    public GameObject TeamBPlayerPrefabVR;

    void Start()
    {
        Debug.Log("ENTER START");

        //instantiate the blue player if team is 0 and red if it is not
        if (NetworkManager.playersTeamA.Contains(PhotonNetwork.LocalPlayer))
        {
            //get a spawn for the correct team
            Transform spawn = SpawnerManager.instance.GetTeamSpawn(0);
           

            GameObject playerPrefabA = UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(TeamAPlayerPrefabPC, TeamAPlayerPrefabVR);


            if (playerPrefabA == null)
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
                    PhotonNetwork.Instantiate("Prefabs/" + playerPrefabA.name, spawn.position, spawn.rotation);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }
        else if(NetworkManager.playersTeamB.Contains(PhotonNetwork.LocalPlayer))
        {
            //get a spawn for the correct team
            Transform spawn = SpawnerManager.instance.GetTeamSpawn(1);


            GameObject playerPrefabA = UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(TeamAPlayerPrefabPC, TeamAPlayerPrefabVR);

            if (playerPrefabA == null)
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
                    PhotonNetwork.Instantiate("Prefabs/" + playerPrefabA.name, spawn.position, spawn.rotation);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

     
    }
}