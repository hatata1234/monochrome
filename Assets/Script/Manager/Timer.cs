using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    private float startTime;
    private bool isRunning = false;
    private float elapsedWhenStopped = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
        elapsedWhenStopped = 0f;
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            elapsedWhenStopped = Time.time - startTime;
            isRunning = false;
        }
    }

    public float GetElapsedTime()
    {
        if (isRunning)
            return Time.time - startTime;
        else
            return elapsedWhenStopped;
    }

    public string GetFormattedTime()
    {
        float t = GetElapsedTime();
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }
}
