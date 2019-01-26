using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Game")]
    public float elapsedTime;

    [Header("Game")]
    public float MatchTime;

    [Header("Game")]
    public GameManager gameManager;

    [Header("Countdown")]
    public int countdownFrom;
    public int currentCountdownTime;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameManager.GetGameState())
        {
            case GameManager.GameState.PLAYING:
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= MatchTime)
                {
                    gameManager.EndGame();
                }
                break;
        }  
    }
}
