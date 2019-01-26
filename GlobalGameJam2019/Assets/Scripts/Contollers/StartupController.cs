using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupController : MonoBehaviour
{

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private string CharacterSelectSceneName;
    [SerializeField] private float StartedJuiceCountdown;
    [SerializeField] private bool GameStarted;

    // Start is called before the first frame update
    void Start()
    {
        if(CharacterSelectSceneName == "")
        {
            Debug.Log("[WARNING] - [StartupController] - Character selection scene not selected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStarted)
        {
            CheckToStartGame();
        }
        else
        {
            ProcessGameStarting();
        }

    }

    void CheckToStartGame()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            GameStarted = true;
            audioManager.PlaySound("ActionOkay");
            Debug.Log("[StartupController] - Starting Juice countdown");
        }
    }

    private void ProcessGameStarting()
    {
        StartedJuiceCountdown -= Time.deltaTime;
        if (StartedJuiceCountdown <= 0.0f)
        {
            Debug.Log("[StartupController] - Loading Scene: " + CharacterSelectSceneName);
            SceneManager.LoadScene(CharacterSelectSceneName);
        }
    }
}
