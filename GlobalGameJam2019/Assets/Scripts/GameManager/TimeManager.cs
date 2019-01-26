using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Match")]
    public GameObject matchTimePanel;
    public Text matchTimeText;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameManager.GetGameState())
        {
            case GameManager.GameState.PLAYING:
                elapsedTime += Time.deltaTime;
                matchTimePanel.SetActive(true);
                matchTimeText.text = ((int)(MatchTime - elapsedTime)).ToString();

                if (elapsedTime >= MatchTime)
                {
                    matchTimePanel.SetActive(false);
                    gameManager.EndGame();
                }
                break;
        }  
    }
}
