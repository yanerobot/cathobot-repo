using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] TimerScript timer;
    [SerializeField] TextMeshProUGUI currentScoreTextObj;
    [SerializeField] TextMeshProUGUI highScoreTextObj;
    [SerializeField] TextMeshProUGUI nextMedalTextObj;
    [SerializeField] GameObject nextMedalMainObj;
    [Header("Medal")]
    [SerializeField] Medal medal;
    [SerializeField] MedalTimesSO medals;

    public static string HighScorePrefsKey => "HighScore" + SceneManager.GetActiveScene().buildIndex;
    public static string HighScoreTextPrefsKey => "HighScoreText" + SceneManager.GetActiveScene().buildIndex;

    void OnEnable()
    {
        var timerInfo = timer.StopTimer();

        var time = timerInfo.Item1;
        var timeText = timerInfo.Item2;

        time = Mathf.Round(time * 1000.0f) / 1000.0f;


        if (time < 1)
        {
            medal.SetMedal(Medal.Type.Current);
            return;
        }

        highScoreTextObj.text = PlayerPrefs.GetString(HighScoreTextPrefsKey, "00:00:000");

        var highScore = PlayerPrefs.GetFloat(HighScorePrefsKey, Mathf.Infinity);

        if (time < highScore)
        {
            PlayerPrefs.SetFloat(HighScorePrefsKey, time);
            PlayerPrefs.SetString(HighScoreTextPrefsKey, timeText);
            Ghost g = FindObjectOfType<Ghost>();
            g?.SaveGhost(true);

            highScoreTextObj.text = timeText;
        }
        else
        {
            time = highScore;
        }

        SetMedal(time);

        currentScoreTextObj.text = timeText;

        var diceObj = FindObjectOfType<Dice>();
        if (diceObj != null)
            diceObj.StopDice();
    }

    void SetMedal(float currentSeconds)
    {
        Medal.Type type;

        if (currentSeconds <= medals.AuthorTime)
            type = Medal.Type.Author;
        else if (currentSeconds <= medals.GoldTime)
            type = Medal.Type.Gold;
        else if (currentSeconds <= medals.SilverTime)
            type = Medal.Type.Silver;
        else if (currentSeconds <= medals.BronzeTime)
            type = Medal.Type.Bronze;
        else
            type = Medal.Type.None;

        var nextMedalTime = GetMedalTime(type);

        if (nextMedalTime != null && PlayerPrefs.GetInt(Medal.CurrentMedalPrefs, -1) < (int)Medal.Type.Author)
            nextMedalTextObj.text = TimerScript.ConvertToTimer((float)nextMedalTime);
        else
            nextMedalMainObj.SetActive(false);

        if ((int)type > PlayerPrefs.GetInt(Medal.CurrentMedalPrefs, -1))
        {
            medal.SetMedal(type);
        }

    }
    
    float? GetMedalTime(Medal.Type type)
    {
        switch (type)
        {
            case Medal.Type.None:
                return medals.BronzeTime;
            case Medal.Type.Bronze:
                return medals.SilverTime;
            case Medal.Type.Silver:
                return medals.GoldTime;
            case Medal.Type.Gold:
                return medals.AuthorTime;
        }

        return null;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
