using System.Collections.Generic;
using Interfaces;
using UI_documents;
using UnityEngine;
using UnityEngine.Events;


public class CaptureTheFlag : MonoBehaviour, IGameBehaviour
{
    [SerializeField] public GameObject capturePointsContainer;
    [SerializeField] public GameObject prefabZoneCapture;
    [SerializeField] public float timerDuration;
    [SerializeField] public CaptureTheFlagScoreUIManager captureTheFlagScoreUIManager;
    [SerializeField] public GameEndUIManager gameEndUIManager;

    public static UnityEvent ChangeCaptureStatus;

    private List<Vector3> _capturePointsLocations;
    private List<ZoneCapture> _capturesZones;

    private bool _timerOn;
    private float _timeLeft;

    private int _scoreTeamA;
    private int _scoreTeamB;

    private void Awake()
    {
        _capturePointsLocations = new List<Vector3>();
        _capturesZones = new List<ZoneCapture>();
        ChangeCaptureStatus = new UnityEvent();
        ChangeCaptureStatus.AddListener(OnZoneCaptured);
        _timeLeft = timerDuration;
    }

    private void Update()
    {
        CheckTimer();
    }

    private void CheckTimer()
    {
        if (_timerOn)
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
            }
            else
            {
                _timerOn = false;
                _timeLeft = timerDuration;
                if (_scoreTeamA > _scoreTeamB)
                {
                    GameOver(AllGenericTypes.Team.TeamA);
                }
                else if (_scoreTeamA < _scoreTeamB)
                {
                    GameOver(AllGenericTypes.Team.TeamB);
                }
                else
                {
                    GameOver(AllGenericTypes.Team.Both);
                }
            }
        }
    }

    private void OnZoneCaptured()
    {
        _scoreTeamA = _scoreTeamB = 0;
        foreach (ZoneCapture zone in _capturesZones)
        {
            if (zone.state == ZoneCapture.State.Captured)
            {
                switch (zone.capturedBy)
                {
                    case AllGenericTypes.Team.TeamA:
                        _scoreTeamA++;
                        break;
                    case AllGenericTypes.Team.TeamB:
                        _scoreTeamB++;
                        break;
                }
            }
        }

        captureTheFlagScoreUIManager.UpdateScore(_scoreTeamA, _scoreTeamB, _capturesZones.Count);
        if (_scoreTeamA == _capturesZones.Count)
        {
            GameOver(AllGenericTypes.Team.TeamA);
        }

        if (_scoreTeamB == _capturesZones.Count)
        {
            GameOver(AllGenericTypes.Team.TeamB);
        }
    }

    public void GameStart()
    {
        _timerOn = true;
    }

    public void GameOver(AllGenericTypes.Team winnerTeam)
    {
        if (winnerTeam == AllGenericTypes.Team.Both || winnerTeam == AllGenericTypes.Team.None)
        {
            gameEndUIManager.gameObject.SetActive(true);
            gameEndUIManager.GameEndDraw(winnerTeam);
        }
        gameEndUIManager.gameObject.SetActive(true);
        gameEndUIManager.GameEndTeamWin(winnerTeam);
    }

    public void OnDisable()
    {
        ChangeCaptureStatus.RemoveListener(OnZoneCaptured);
    }

    public void SetUpGame()
    {
        ARPointManager.Point[] captureZones = ARPointManager.Instance.aRPoints.captureZones;

        foreach (ARPointManager.Point captureZone in captureZones)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(captureZone.x, 50, captureZone.z), Vector3.down, out hit))
            {
                _capturePointsLocations.Add(new Vector3(captureZone.x, hit.point.y, captureZone.z));
            }
            else
            {
                _capturePointsLocations.Add(new Vector3(captureZone.x, -10.52f, captureZone.z));
            }
        }
        captureTheFlagScoreUIManager.gameObject.SetActive(true);
        foreach (Vector3 capturePointLocation in _capturePointsLocations)
        {
            GameObject tempPrefabZoneCapture =
                Instantiate(prefabZoneCapture, capturePointLocation, Quaternion.identity);
            tempPrefabZoneCapture.transform.parent = capturePointsContainer.transform;
            ZoneCapture zoneCapture = tempPrefabZoneCapture.GetComponent<ZoneCapture>();
            _capturesZones.Add(zoneCapture);
        }
    }
}