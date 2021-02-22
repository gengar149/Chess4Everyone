using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer
{
    private float timeRemaining;
    private bool timerIsRunning = false;
    private TMP_Text clock;

    public void Setup(float timeMax, TMP_Text display)
    {
        timeRemaining = timeMax;
        clock = display;
    }


    // Start is called before the first frame update
    public void Start()
    {
        timerIsRunning = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out !");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float tenthOfSecond;
        if(seconds < 20)
        {
            tenthOfSecond = Mathf.FloorToInt( (timeToDisplay % 1) * 10);
            clock.text = string.Format("0:{0:00}:{1:0}", seconds, tenthOfSecond);

        } else
        {
            clock.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
    }
}
