/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTaroDev : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject ghostPrefab;
    [SerializeField, Range(1,10)] int captureEveryNFrame;

    ReplaySystem replaySystem;

    void Awake()
    {
        replaySystem = new ReplaySystem(this);

        LevelStartCountDown.OnCountDownEnd.AddListener(OnGameStart);
    }

    bool finished;

    void Update()
    {
        if (UIBehaiv.LevelEnded && !finished)
        {
            finished = true;
            OnLevelEnded();
        }
    }

    void OnGameStart()
    {
        replaySystem.StartRun(target, captureEveryNFrame);
        replaySystem.PlayRecording(RecordingType.Best, Instantiate(ghostPrefab));
    }

    void OnLevelEnded()
    {
        replaySystem.FinishRun();
        replaySystem.StopReplay();
    }
}
*/