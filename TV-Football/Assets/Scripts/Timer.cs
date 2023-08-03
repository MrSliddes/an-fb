using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays left game time
/// </summary>
public class Timer : MonoBehaviour
{
    public Values values;
    public Components components;

    /// <summary>
    /// Has the timer been started?
    /// </summary>
    private bool startedTimer;
    /// <summary>
    /// Current time left
    /// </summary>
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
            components.timerField.text = time > 0 ? $"Time Left: {(int)time}" : "Time's Up!";
        }
    }

    public void StartTimer()
    {
        time = values.countDown;
        startedTimer = true;
    }

    [System.Serializable]
    public class Components
    {
        [Tooltip("Reference to timer field for displaying time left")]
        public TextMeshProUGUI timerField;
    }

    [System.Serializable]
    public class Values
    {
        [Tooltip("How long the timer is")]
        public float countDown = 30;
    }
}
