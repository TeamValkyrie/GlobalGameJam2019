using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour
{
    [Header("Countdown")]
    public Text countDownText;
    public GameObject countDownPanel;
    public string finalText = "GO!";

    [Header("Paused")]
    [SerializeField] private GameObject pausedPanel;

    [Header("Options")]
    [SerializeField] private GameObject optionsCanvas;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        if (!countDownText)
        {
            Debug.LogWarning("Failed to find countdown text!");
        }

        if (!countDownPanel)
        {
            Debug.LogWarning("Failed to find countdown panel!");
        }

        eventSystem = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleOptions(bool active = false)
    {
        optionsCanvas.SetActive(active);

        if (optionsCanvas.activeInHierarchy)
        {
            eventSystem.SetSelectedGameObject(masterSlider.gameObject);
        }
        else
        {
            eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        }
    }

    public void TogglePause(bool active = false)
    {
        pausedPanel.SetActive(active);
    }
}
