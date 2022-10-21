using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Unity.VisualScripting;


public class ZoneCapture : MonoBehaviourPunCallbacks, IPunObservable
{
    private List<PlayerTeam> _playerTeamA;
    private List<PlayerTeam> _playerTeamB;
    

    public enum State
    {
        Neutral,
        Captured
    };

    public State state;
    [SerializeField] private float progressSpeed = 0.5f;

    public AllGenericTypes.Team capturedBy;

    private float _progress;

    public TextMeshProUGUI battle;

    private ParticleSystem ps;

    // public string MyString = string.Empty;

    // Start is called before the first frame update
    void Awake()
    {
        state = State.Neutral;
        _playerTeamB = new List<PlayerTeam>();
        _playerTeamA = new List<PlayerTeam>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UI.Instance.battleText.gameObject.SetActive(false);
        // if (state == State.Captured) return;

        if (_playerTeamB.Count > 0 && _playerTeamA.Count > 0)
        {
            UI.Instance.battleText.gameObject.SetActive(true);
            return;
        }

        if (_playerTeamA.Count + _playerTeamB.Count <= 0 && state != State.Captured)
        {
            _progress = 0;
            return;
        }
        
        if (_playerTeamB.Count > 0)
        {
            if (_progress <= -1f)
            {
                ps = GetComponentInChildren<ParticleSystem>();
                var main = ps.main;

                state = State.Captured;
                main.startColor = Color.blue;
                GetComponentInChildren<Renderer>().material.color = Color.blue;
                capturedBy = AllGenericTypes.Team.TeamB;
                CaptureTheFlag.ChangeCaptureStatus.Invoke();
                return;
            }

            UI.Instance.flagZoneCaptureProgressCanvas.color = Color.blue;
            _progress -= _playerTeamB.Count * progressSpeed * Time.deltaTime;
            UI.Instance.flagZoneCaptureProgressCanvas.fillAmount = -_progress;
            
        }
        else if (_playerTeamA.Count > 0)
        {
            if (_progress >= 1f)
            {
                ps = GetComponentInChildren<ParticleSystem>();
                var main = ps.main;

                state = State.Captured;
                main.startColor = Color.red;
                GetComponentInChildren<Renderer>().material.color = Color.red;
                capturedBy = AllGenericTypes.Team.TeamA;
                CaptureTheFlag.ChangeCaptureStatus.Invoke();
                return;
            }

            UI.Instance.flagZoneCaptureProgressCanvas.color = Color.red;
            _progress += _playerTeamA.Count * progressSpeed * Time.deltaTime;
            UI.Instance.flagZoneCaptureProgressCanvas.fillAmount = _progress;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.root.TryGetComponent(out PlayerTeam playerTeam))
        {
            if (!_playerTeamA.Contains(playerTeam) && playerTeam.team == AllGenericTypes.Team.TeamA) _playerTeamA.Add(playerTeam);
            if (!_playerTeamB.Contains(playerTeam) && playerTeam.team == AllGenericTypes.Team.TeamB) _playerTeamB.Add(playerTeam);

            if (other.transform.root.gameObject.GetPhotonView().IsMine)
            {
                UI.Instance.flagZoneCaptureProgressCanvas.gameObject.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.transform.root.TryGetComponent(out PlayerTeam playerTeam))
        {
            if (_playerTeamA.Contains(playerTeam)) _playerTeamA.Remove(playerTeam);
            if (_playerTeamB.Contains(playerTeam)) _playerTeamB.Remove(playerTeam);

            if (other.transform.root.gameObject.GetPhotonView().IsMine)
            {
                UI.Instance.flagZoneCaptureProgressCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_progress);
            // stream.SendNext(GetPlayerMapAreas());
            stream.SendNext(GetComponent<Renderer>().material.color);
        }
        else
        {
            _progress = (int)stream.ReceiveNext();
            // playerMapAreasList = (List<PlayerMapAreas>)stream.ReceiveNext();
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