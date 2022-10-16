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
        
    }
    
}