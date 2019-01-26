using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [HideInInspector]
    public enum GameState { NONE, SPAWNING, COUNTING, PLAYING, PAUSED, OPTIONS };

    [HideInInspector]
    public AudioManager audioManager;

    [HideInInspector]
    public TimeManager timeManager;

    [HideInInspector]
    public CanvasManager canvasManager;

    [HideInInspector]
    public PlayerManager playerManager;

    [SerializeField]
    private GameState gameState = GameState.NONE;

    [SerializeField]
    private string MenuMusicName;

    [SerializeField]
    private string BattleMusicName;


    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        timeManager = FindObjectOfType<TimeManager>();
        canvasManager = FindObjectOfType<CanvasManager>();
        playerManager = FindObjectOfType<PlayerManager>();

        SetGameState(GameState.COUNTING);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            if (gameState == GameState.PLAYING)
            {
                SetGameState(GameState.PAUSED);
            }
            else if (gameState == GameState.PAUSED)
            {
                SetGameState(GameState.PLAYING);
            }
            else if (gameState == GameState.OPTIONS)
            {
                SetGameState(GameState.PAUSED);
            }
        }

        if (Input.GetButtonUp("Submit"))
        {
            if (gameState == GameState.PAUSED)
            {
                SetGameState(GameState.OPTIONS);
            }
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;

        switch (state)
        {
            case GameState.SPAWNING:
                break;
            case GameState.COUNTING:
                StartCoroutine(StartCountdown());
                break;
            case GameState.PLAYING:
                canvasManager.ToggleOptions(false);
                canvasManager.TogglePause(false);
                audioManager.PlayMusic(BattleMusicName);
                break;
            case GameState.PAUSED:
                canvasManager.ToggleOptions(false);
                canvasManager.TogglePause(true);
                audioManager.PauseMusic(BattleMusicName);
                break;
            case GameState.OPTIONS:
                canvasManager.ToggleOptions(true);
                canvasManager.TogglePause(false);
                break;
            default:
                break;
        }
    }

    private IEnumerator StartCountdown()
    {
        timeManager.currentCountdownTime = timeManager.countdownFrom;
        canvasManager.countDownPanel.SetActive(true);
        canvasManager.countDownText.text = timeManager.currentCountdownTime.ToString();
        audioManager.PlaySound("Countdown");

        while (timeManager.currentCountdownTime > 0)
        {
            Debug.Log("Countdown: " + timeManager.currentCountdownTime);
            yield return new WaitForSeconds(1.0f);
            timeManager.currentCountdownTime--;
            canvasManager.countDownText.text = timeManager.currentCountdownTime.ToString();
        }

        canvasManager.countDownText.text = canvasManager.finalText;
        yield return new WaitForSeconds(1.0f);
        canvasManager.countDownPanel.SetActive(false);
        SetGameState(GameState.PLAYING);
    }
}
