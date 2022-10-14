using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCapture : MonoBehaviour
{
    private List<PlayerMapAreas> playerMapAreasList = new List<PlayerMapAreas>();
    private List<PlayerMapAreas> playerMapAreasListTeamA = new List<PlayerMapAreas>();
    private List<PlayerMapAreas> playerMapAreasListTeamB = new List<PlayerMapAreas>();

    public enum State {Neutral, Captured};
    private State state;

    private float progress;
    private float progress_A;
    private float progress_B;
    private bool isCapturing = false;

    // Start is called before the first frame update
    void Awake()
    {
        state = State.Neutral;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.Neutral:

                //List<PlayerMapAreas> playerMapAreasInsideList = new List<PlayerMapAreas>();

                foreach (PlayerMapAreas playerMapAreas in GetPlayerMapAreas())
                {
                    if (!playerMapAreasList.Contains(playerMapAreas))
                    {
                        playerMapAreasList.Add(playerMapAreas);
                        Debug.Log("Players inside: " + playerMapAreasList.Count);

                        if (playerMapAreas.gameObject.layer == LayerMask.NameToLayer("TeamA"))
                        {
                            playerMapAreasListTeamA.Add(playerMapAreas);
                            Debug.Log("Players A inside: " + playerMapAreasListTeamA.Count);

                        }
                    }
                }

                
                float progressSpeed = 0.5f;
                progress_A += playerMapAreasListTeamA.Count * progressSpeed * Time.deltaTime;

                Debug.Log("Inside zone: " + playerMapAreasListTeamA.Count + " ; progress A: " + progress_A);

                if (progress_A >= 1f)
                {
                    state = State.Captured;
                    GetComponent<Renderer>().material.color = Color.red;
                }

                 if (progress_A >= 0 && playerMapAreasListTeamA.Count == 0)
                {
                    progress_A -= progressSpeed * Time.deltaTime;
                    Debug.Log("Leaved during progress, Player inside zone: " + playerMapAreasListTeamA.Count + " ; progress: " + progress_A);
                }
                
                break;
            case State.Captured:
                break;
        }


        //int playersCountInside = 0;
        //playersCountInside += GetPlayerMapAreas().Count;

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerMapAreas>(out PlayerMapAreas playerMapAreas))
        {
            Debug.Log("entered zone: " + other.name);
            playerMapAreasList.Add(playerMapAreas);
        }

        isCapturing = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMapAreas>(out PlayerMapAreas playerMapAreas))
        {
            Debug.Log("exit zone");
            playerMapAreasList.Remove(playerMapAreas);
        }

        isCapturing = false;
    }

    public List<PlayerMapAreas> GetPlayerMapAreas()
    {
        return playerMapAreasList;
    }
}
