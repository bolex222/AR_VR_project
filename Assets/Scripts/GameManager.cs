using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public enum GameModeOptions {DeathMatch, CaptureTheFlag}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameModeOptions gameMode;

    private IGameBehaviour _game;
    
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
        _game.SetUpGame();
        _game.GameStart();
    }
}
