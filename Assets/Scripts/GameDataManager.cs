using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    private string jsonPath;

    [System.Serializable]
    public class GameData
    {
        public int LifeNumber;
        public float DelayShot;
        public float DelayTeleport;
        public float DelayRespawn;
        public string ColorShotVirus;
        public string ColorShotKMS;
        public int NbContaminedPlayerToVictory;
        public float RadiusExplosion;
        public int TimeToAreaContamination;
    }

    public GameData data = new GameData();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        jsonPath = $"{Application.dataPath}/StreamingAssets/GameConfig.json";

        try
        {
            if (File.Exists(jsonPath))
            {
                getData(jsonPath);
                SceneManager.LoadScene("Lobby");
                Debug.Log("Data loaded");
            }
        }
        catch (IOException e)
        {
            // Extract some information from this exception, and then
            // throw it to the parent method.
            if (e.Source != null)
                Console.WriteLine("IOException source: {0}", e.Source);
            throw;
        }
    }

    private void getData(string path)
    {
        string jsonContent = File.ReadAllText(jsonPath);
        data = JsonUtility.FromJson<GameData>(jsonContent);
    }

    
}
