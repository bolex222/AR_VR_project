using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    //the instnce on the scene
    public static SpawnerManager instance;

    //the team spawn
    public List<GameObject> redTeamSpawns;
    public List<GameObject> blueTeamSpawns;

    // Start is called before the first frame update
    void Awake()
    {
        //recreate the instance on awake- ie if scene reloads
        instance = this;
        /*redTeamSpawns = GameObject.FindGameObjectsWithTag("RedSpawn");
        blueTeamSpawns = GameObject.FindGameObjectsWithTag("BlueSpawn");*/
        redTeamSpawns = new List<GameObject>();
        blueTeamSpawns = new List<GameObject>();
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     photonView.RPC("SetSpawnPointFromJson", RpcTarget.AllViaServer);
        // }
        SetSpawnPointFromJson();
    }

    // [PunRPC]
    private void SetSpawnPointFromJson()
    {
        ARPointManager arPointManager = ARPointManager.Instance;
        List<List<GameObject>> myArray = new List<List<GameObject>>
        {
            redTeamSpawns,
            blueTeamSpawns
        };
        RaycastHit hit;
        for (var i = 0; i < myArray.Count; i++)
        {
            foreach (ARPointManager.Point point in (i > 0
                         ? arPointManager.aRPoints.spawnPointsTeamB
                         : arPointManager.aRPoints.spawnPointsTeamA))
            {
                GameObject spawnPoint = new GameObject("spawn")
                {
                    transform =
                    {
                        localPosition = Physics.Raycast(new Vector3(point.x, 50, point.z), Vector3.down, out hit)
                            ? new Vector3(point.x, hit.point.y, point.z)
                            : new Vector3(point.x, 0f, point.z),
                        parent = gameObject.transform
                    }
                };
                myArray[i].Add(spawnPoint);
            }
        }
    }


    public Transform GetRandomRedSpawn()
    {
        //return a transform for one of the red spawns
        return redTeamSpawns[Random.Range(0, redTeamSpawns.Count)].transform;
        //return redTeamSpawns[0].transform;
    }

    public Transform GetRandomBlueSpawn()
    {
        //return a transform for one of the red spawns
        return blueTeamSpawns[Random.Range(0, redTeamSpawns.Count)].transform;
        //return blueTeamSpawns[0].transform;
    }

//this method gets given the team number to find a spawn for
    public Transform GetTeamSpawn(int teamNumber)
    {
        return teamNumber == 0 ? GetRandomBlueSpawn() : GetRandomRedSpawn();
    }
}