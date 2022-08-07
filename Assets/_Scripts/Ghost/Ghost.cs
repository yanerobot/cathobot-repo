using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Text;

public class Ghost : MonoBehaviour
{
    [SerializeField] float frequency;
    [SerializeField] Health health;
    [SerializeField] Transform mainTransform;
    [SerializeField] Transform gfxTransform;
    [SerializeField] PowerUp powerUp;
    [SerializeField] string replaysPath;
    [SerializeField] SpriteRenderer sr;

    List<GhostFrameData> ghostDataCurrent;
    List<GhostFrameData> ghostDataHighScore;

    float currentTime;
    float currentDelta;

    bool recording;

    GhostFrameData currentGFD;
    GhostFrameData prevGFD;

    int currentGDindex;
    bool isLastFrame;

    bool loaded;
    string FileName => "/HighScoreScene" + SceneManager.GetActiveScene().buildIndex + ".save";

    void Start()
    {
        ghostDataCurrent = new List<GhostFrameData>();

        sr.enabled = false;

        loaded = Serializer.TryLoad(out ghostDataHighScore, FileName, replaysPath);

        SafeZone.OnSafeZoneOut.AddListener(EnableRecording);
    }

    void EnableRecording()
    {
        if (loaded)
        {
            sr.enabled = true;
            transform.position = ghostDataHighScore[0].position;
            transform.rotation = ghostDataHighScore[0].rotation;
            currentGDindex = 1;
        }

        recording = true;
    }

    void Update()
    {
        if (Time.timeScale == 0 || !recording || health.isDead)
            return;

        if (UIBehaiv.LevelEnded)
        {
            recording = false;
            return;
        }

        currentTime += Time.unscaledDeltaTime;
        currentDelta += Time.unscaledDeltaTime;

        if (currentDelta >= frequency)
        {
            currentDelta -= frequency;
            Record();
            if (loaded)
                SetCurrentGhostFrameDataIndex();
        }
        if (!isLastFrame)
            UpdateGhostValues();
        else
            sr.enabled = false;
    }

    public void SaveGhost(bool highScore = false)
    {
        if (highScore)
            Serializer.Save(ghostDataCurrent, FileName, replaysPath);
    }

    void SetCurrentGhostFrameDataIndex()
    {
        if (currentGDindex >= ghostDataHighScore.Count)
        {
            isLastFrame = true;
            return;
        }

        var ghostTime = ghostDataHighScore[currentGDindex].timeValue;

        if (ghostTime - currentTime > frequency * 0.5f || ghostTime - currentTime < frequency * 1.5f)
        {
            currentGFD = ghostDataHighScore[currentGDindex];
            prevGFD = ghostDataHighScore[currentGDindex - 1];
            currentGDindex++;
        }
        else
        {
            currentGDindex++;
            SetCurrentGhostFrameDataIndex();
        }
    }

    void UpdateGhostValues()
    {
        if (currentGFD == null)
            return;

        transform.position = Vector3.Slerp(prevGFD.position, currentGFD.position, currentDelta * (1/frequency));
        transform.rotation = Quaternion.Slerp(prevGFD.rotation, currentGFD.rotation, currentDelta * (1 / frequency));

        transform.localScale = currentGFD.gfxSize;
    }

    void Record()
    {
        var frameData = new GhostFrameData(
                currentTime,
                mainTransform.position,
                mainTransform.rotation,
                gfxTransform.localScale,
                powerUp.currentBuff
            );

        ghostDataCurrent.Add(frameData);
    }
}

[Serializable]
public class GhostFrameData
{
    public int currentBuff;
    public float timeValue;
    public SerializableVector3 position;
    public SerializableVector3 gfxSize;
    public SerializableQuaternion rotation;

    public GhostFrameData(float timeValue, Vector3 position, Quaternion rotation, Vector3 gfxSize, int currentBuff)
    {
        this.timeValue = timeValue;
        this.position = position;
        this.rotation = rotation;
        this.gfxSize = gfxSize;
        this.currentBuff = currentBuff;
    }
}
