using System;
using System.IO;
using UnityEngine;

public class ARPointManager: MonoBehaviour
{
    public static ARPointManager Instance { get; private set; }
    
    [Serializable]
    public class Point
    {
        public float x;
        public float y;
        public float z;
    }
    
    [Serializable]
    public class ARPoints
    {
        public Point[] captureZones;
        public Point[] throwable;
        public Point[] spawnPointsTeamA;
        public Point[] spawnPointsTeamB;
    }

    public ARPoints aRPoints;

    private void Awake()
    {

        string jsonPath = $"{Application.dataPath}/StreamingAssets/Level.json";
        Instance = this;
        aRPoints = new ARPoints();

        try
        {
            if (File.Exists(jsonPath))
            {
                string jsonLevelContent = File.ReadAllText(jsonPath);
                ARPoints values = JsonUtility.FromJson<ARPoints>(jsonLevelContent);
                aRPoints = values;
            }
        }
        catch (IOException e)
        {
            if (e.Source != null)
                Console.WriteLine("IOException source: {0}", e.Source);
            throw;
        }
    }
}