using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;


public class ZoneCapture : MonoBehaviourPunCallbacks, IPunObservable
{
    private List<PlayerMapAreas> playerMapAreasList;
    private List<PlayerMapAreas> playerMapAreasListTeamA;
    private List<PlayerMapAreas> playerMapAreasListTeamB;

    public enum State {Neutral, Captured};
    public State state;

    public AllGenericTypes.Team capturedBy;

    private float progress;
    private float progress_A;
    private float progress_B;

    private bool isCapturing;

    public TextMeshProUGUI battle;

    // public string MyString = string.Empty;

    // Start is called before the first frame update
    void Awake()
    {
        state = State.Neutral;
        playerMapAreasList = new List<PlayerMapAreas>();
        playerMapAreasListTeamA = new List<PlayerMapAreas>();
        playerMapAreasListTeamB = new List<PlayerMapAreas>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Neutral:

                foreach (PlayerMapAreas playerMapAreas in GetPlayerMapAreas())
                {
                    if (!playerMapAreasList.Contains(playerMapAreas) &&
                        playerMapAreas.gameObject.GetPhotonView().IsMine == true)
                    {
                        playerMapAreasList.Add(playerMapAreas);

                        if (playerMapAreas.gameObject.layer == LayerMask.NameToLayer("TeamA"))
                        {
                            playerMapAreasListTeamA.Add(playerMapAreas);
                        }
                    }
                    else if (playerMapAreasList.Contains(playerMapAreas) &&
                             playerMapAreas.gameObject.GetPhotonView().IsMine == true)
                    {
                        if (playerMapAreas.gameObject.layer == LayerMask.NameToLayer("TeamA") &&
                            !playerMapAreasListTeamA.Contains(playerMapAreas))
                        {
                            playerMapAreasListTeamA.Add(playerMapAreas);
                        }
                        else if (playerMapAreas.gameObject.layer == LayerMask.NameToLayer("TeamB") &&
                                 !playerMapAreasListTeamB.Contains(playerMapAreas))
                        {
                            playerMapAreasListTeamB.Add(playerMapAreas);
                        }
                    }
                }

                if (playerMapAreasListTeamA.Count > 0)
                {
                    float progressSpeed = 0.5f;
                    UI.Instance.flagZoneCaptureProgressCanvas.color = Color.red;

                    progress += playerMapAreasListTeamA.Count * progressSpeed * Time.deltaTime;
                    UI.Instance.flagZoneCaptureProgressCanvas.fillAmount = progress;


                    if (progress >= 1f)
                    {
                        state = State.Captured;
                        GetComponent<Renderer>().material.color = Color.red;
                        capturedBy = AllGenericTypes.Team.TeamA;
                        CaptureTheFlag.ChangeCaptureStatus.Invoke();
                    }

                    if (progress >= 0 && playerMapAreasListTeamA.Count == 0)
                    {
                        progress = 0;
                    }
                }

                else if (playerMapAreasListTeamB.Count > 0)
                {
                    float progressSpeed = 0.5f;
                    UI.Instance.flagZoneCaptureProgressCanvas.color = Color.blue;

                    progress -= playerMapAreasListTeamB.Count * progressSpeed * Time.deltaTime;
                    UI.Instance.flagZoneCaptureProgressCanvas.fillAmount = -progress;

                    if (progress <= -1f)
                    {
                        state = State.Captured;
                        GetComponent<Renderer>().material.color = Color.blue;
                        capturedBy = AllGenericTypes.Team.TeamB;
                        CaptureTheFlag.ChangeCaptureStatus.Invoke();
                    }

                    if (progress >= 0 && playerMapAreasListTeamB.Count == 0)
                    {
                        progress = 0;
                    }
                }

                else if (playerMapAreasListTeamA.Count > 0 && playerMapAreasListTeamB.Count > 0)
                {
                    battle.gameObject.SetActive(true);
                }


                break;
            case State.Captured:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMapAreas>(out PlayerMapAreas playerMapAreas))
        {
            Debug.Log("entered zone: " + other.gameObject.GetPhotonView().ViewID);
            playerMapAreasList.Add(playerMapAreas);
            UI.Instance.flagZoneCaptureProgressCanvas.gameObject.SetActive(true);
        }

        isCapturing = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMapAreas>(out PlayerMapAreas playerMapAreas))
        {
            if (other.gameObject.GetPhotonView().IsMine == true)
            {
                Debug.Log("exited zone: " + other.gameObject.GetPhotonView().ViewID);
                playerMapAreasList.Remove(playerMapAreas);
                UI.Instance.flagZoneCaptureProgressCanvas.gameObject.SetActive(false);
            }
        }

        isCapturing = false;
    }

    public List<PlayerMapAreas> GetPlayerMapAreas()
    {
        return playerMapAreasList;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(progress);
            stream.SendNext(GetPlayerMapAreas());
            stream.SendNext(GetComponent<Renderer>().material.color);
        }
        else
        {
            progress = (int)stream.ReceiveNext();
            playerMapAreasList = (List<PlayerMapAreas>)stream.ReceiveNext();
            GetComponent<Renderer>().material.color = (Color)stream.ReceiveNext();
        }
    }

    // [PunRPC]
    // public void CaptureProgress(float progressCoef, float progressValue, float captureCondition,
    //     List<PlayerMapAreas> teamList, Color teamColor, Image progressUI, int teamNumber)
    // {
    //     progressUI.color = teamColor;
    //
    //     if (teamNumber == 1)
    //     {
    //         progressValue += teamList.Count * progressCoef * Time.deltaTime;
    //     }
    //     else if (teamNumber == 2)
    //     {
    //         progressValue -= teamList.Count * progressCoef * Time.deltaTime;
    //     }
    //
    //     progressImage.fillAmount = progress;
    //
    //
    //     if (progressImage.fillAmount == captureCondition)
    //     {
    //         state = State.Captured;
    //         GetComponent<Renderer>().material.color = teamColor;
    //     }
    //
    //     if (progressValue > 0 && teamList.Count == 0)
    //     {
    //         progressValue = 0;
    //     }
    // }
}