using UnityEngine;
using System;
using TMPro;
using System.Text;

public class TimerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float currentSeconds;

    bool isStopped = true;

    StringBuilder sb;

    void Start()
    {
        isStopped = true;
        sb = new StringBuilder();
        SafeZone.OnSafeZoneOut.AddListener(StartTimer);

        currentSeconds = 0;
        timerText.text = "00:00:000";
    }

    void Update()
    {
        if (Time.timeScale == 0 || isStopped)
            return;

        currentSeconds += Time.unscaledDeltaTime;

        timerText.text = ConvertToTimer(currentSeconds, sb);
    }

    public static string ConvertToTimer(float seconds, StringBuilder sb = null)
    {
        if (sb == null)
            sb = new StringBuilder();

        var curMinutes = Mathf.Floor(seconds / 60);
        var curSeconds = Mathf.Floor(seconds % 60);
        var curMilliSeconds = seconds * 1000 % 1000;

        sb.Clear();
        sb.Append(curMinutes.ToString("00")).Append(":")
            .Append(curSeconds.ToString("00")).Append(":")
            .Append(curMilliSeconds.ToString("000"));

        return sb.ToString();
    }

    public (float, string) StopTimer()
    {
        isStopped = true;

        return (currentSeconds, timerText.text);
    }

    void StartTimer()
    {
        isStopped = false;
    }
}
