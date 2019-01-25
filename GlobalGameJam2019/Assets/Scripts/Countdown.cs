using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField]
    private string finalText = "GO!";

    [SerializeField]
    private int countdownFrom;

    [SerializeField]
    private Text countdownText;

    private int currentTime;

    // Start is called before the first frame update
    void Start()
    {
        countdownText = GetComponent<Text>();

        if (countdownText == null)
        {
            Debug.LogError("No countdown text object referenced!");
        }

        currentTime = countdownFrom;
        countdownText.text = currentTime.ToString();

        StartCoroutine(StartCountdown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TimeUp()
    {
        countdownText.text = finalText;
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator StartCountdown()
    {
        while (currentTime > 0)
        {
            Debug.Log("Countdown: " + currentTime);
            yield return new WaitForSeconds(1.0f);
            currentTime--;
            countdownText.text = currentTime.ToString();
        }

        TimeUp();
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
