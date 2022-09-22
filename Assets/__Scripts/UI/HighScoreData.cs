using System;

[Serializable]
public struct HighScoreData
{
    public float time;
    public string timeText;
    public int medal;

    public HighScoreData(float newTime, string newTimeText, int newMedal)
    {
        time = newTime;
        timeText = newTimeText;
        medal = newMedal;
    }
}
