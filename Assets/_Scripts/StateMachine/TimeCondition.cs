﻿using UnityEngine;

public class TimeCondition
{
    float startTime;
    float timeLimit;

    public TimeCondition(float time)
    {
        timeLimit = time;
        startTime = Time.time - timeLimit;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
    }

    public bool HasTimePassed()
    {
        return Time.time - startTime >= timeLimit;
    }
}
