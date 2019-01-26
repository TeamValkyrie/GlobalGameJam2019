using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Countdown")]
    public Text countDownText;
    public GameObject countDownPanel;
    public string finalText = "GO!";

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
