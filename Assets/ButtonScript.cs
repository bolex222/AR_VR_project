using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private MatchMakingUi matchMakingUi;

    public void OnStartGame()
    {
        matchMakingUi.OnStartGame();
    }
    
}
