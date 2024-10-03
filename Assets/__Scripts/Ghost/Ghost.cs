using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ghost : MonoBehaviour
{
    [SerializeField, Range(1, 10), Tooltip("Frames")] int recordFrequency = 1;
    [SerializeField] Health health;
    [SerializeField] Transform mainTransform;
    [SerializeField] Transform gfxTransform;
    [SerializeField] PowerUp powerUp;
    [SerializeField] SpriteRenderer sr;

    List<GhostFrameData> ghostDataCurrent;
    List<GhostFrameData> ghostDataHighScore;

    float currentTime;
    float currentFrame;

    bool recording;

    GhostFrameData currentGFD;
    GhostFrameData prevGFD;

    int currentGFDindex;
    bool isLastFrame;
    bool isReplayFinished;

    bool loaded;
    string Prefs_Key => "HighScoreReplay_" + SceneManager.GetActiveScene().name + "_BI_" + SceneManager.GetActiveScene().buildIndex;
    //string ReplaysPath => GameManager.SAVE_FILES_DIRECTORY + "Replays/";

    void Start()
    {
        ghostDataCurrent = new List<GhostFrameData>();

        sr.enabled = false;

        var loadedData = PlayerPrefs.GetString(Prefs_Key, null);
        loaded = Serializer.TryDeserializeString(loadedData, out ghostDataHighScore); //Serializer.TryLoad(out ghostDataHighScore, FileName, ReplaysPath);

        SafeZone.OnSafeZoneExit.AddListener(StartGhost);
    }

    void StartGhost()
    {
        if (loaded)
        {
            sr.enabled = true;
            transform.position = ghostDataHighScore[0].position;
            transform.rotation = ghostDataHighScore[0].rotation;
            currentGFDindex = 1;
        }

        recording = true;
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0 || !recording || health.isDead)
            return;

        if (UIBehaiv.LevelEnded)
        {
            recording = false;
            return;
        }

        if (isReplayFinished)
        {
            sr.enabled = false;
            return;
        }

        if (currentFrame % recordFrequency == 0)
        {
            Record();
            if (loaded)
                SetCurrentGhostFrame();
        }

        currentFrame++;

        UpdateGhostValues();
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
    }

    public void SaveGhost(bool highScore = false)
    {
        if (highScore)
        {
            var ghostData = Serializer.SerializeToString(ghostDataCurrent);
            PlayerPrefs.SetString(Prefs_Key, ghostData); //Serializer.Save(ghostDataCurrent, FileName, ReplaysPath);
        }
    }

    void SetCurrentGhostFrame()
    {
        if (currentGFDindex >= ghostDataHighScore.Count - 1)
        {
            isLastFrame = true;
            return;
        }

        for (int i = currentGFDindex; i < ghostDataHighScore.Count; i++)
        {
            if (ghostDataHighScore[i].timeValue > currentTime)
            {
                currentGFD = ghostDataHighScore[i];
                prevGFD = ghostDataHighScore[i - 1];
                currentGFDindex = i;
                break;
            }
        }
    }

    void UpdateGhostValues()
    {
        if (currentGFD == null)
            return;

        var lerpValue = (currentTime - prevGFD.timeValue) / (currentGFD.timeValue - prevGFD.timeValue);

        transform.position = Vector3.Lerp(prevGFD.position, currentGFD.position, lerpValue);
        transform.rotation = Quaternion.Lerp(prevGFD.rotation, currentGFD.rotation, lerpValue);

        transform.localScale = currentGFD.gfxSize;

        if (isLastFrame && lerpValue >= 1)
        {
            isReplayFinished = true;
        }
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
