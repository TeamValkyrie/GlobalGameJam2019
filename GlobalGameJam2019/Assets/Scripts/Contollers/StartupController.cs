using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartupController : MonoBehaviour
{
    [HideInInspector] public AudioManager audioManager;
    [SerializeField] private string CharacterSelectSceneName;
    [SerializeField] private float StartedJuiceCountdown;
    [SerializeField] private bool GameStarted;
    [SerializeField] private GameObject optionsCanvas;

    private EventSystem eventSystem;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        eventSystem = FindObjectOfType<EventSystem>();

        if (CharacterSelectSceneName == "")
        {
            Debug.Log("[WARNING] - [StartupController] - Character selection scene not selected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStarted)
        {
            ProcessGameStarting();
        }

        if (Input.GetButtonUp("Cancel"))
        {
            OnOptions();
        }
    }

    public void StartGame()
    {
        GameStarted = true;
        Debug.Log("[StartupController] - Starting Juice countdown");
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void MainVolumeSliderChange()
    {
        audioManager.SetMasterVolume(masterSlider.value);
    }

    public void MusicVolumeSliderChange()
    {
        audioManager.SetMusicVolume(musicSlider.value);
    }

    public void SFXVolumeSliderChange()
    {
        audioManager.SetSoundEffectsVolume(sfxSlider.value);
    }

    public void OnOptions()
    {
        optionsCanvas.SetActive(!optionsCanvas.activeInHierarchy);

        if (optionsCanvas.activeInHierarchy)
        {
            eventSystem.SetSelectedGameObject(masterSlider.gameObject);
        }
        else
        {
            eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        }
    }
}
