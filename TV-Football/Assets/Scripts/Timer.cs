using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float countDown = 30;
    public TextMeshProUGUI timerField;

    private bool startedTimer;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(startedTimer)
        {
            time -= Time.deltaTime;
            if(time <= 0) time = 0;
            timerField.text = time > 0 ? $"Time Left: {(int)time}" : "Time's Up!";
        }
    }

    public void StartTimer()
    {
        time = countDown;
        startedTimer = true;
    }
}
