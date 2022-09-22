using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIBehaiv : MonoBehaviour
{
    [SerializeField] GameObject gameOverGO;
    [SerializeField] GameObject levelCompletedGO;

    Health playerHealth;

    public static bool LevelEnded;

    public static UnityAction OnShowLevelHints; // Should be subscribed from Awake
    public bool LevelHintsDisabled { get; set; }


    void Awake()
    {
        LevelEnded = false;
        gameOverGO.SetActive(false);
        var player = GameObject.FindWithTag(TopDownMovement.PLAYERTAG);

        playerHealth = player.GetComponent<Health>();
        playerHealth._OnDie.AddListener(EnableGameOverUI);
    }

    void Start()
    {
        var loadData = PlayerPrefs.GetString(EndGameScreen.Prefs_Key, null);

        //Serializer.TryLoad(out HighScoreData data, EndGameScreen.HighScoreData_FileName, EndGameScreen.HighScoreData_Path)
        if (Serializer.TryDeserializeString(loadData, out HighScoreData data) && !LevelHintsDisabled)
        {
            if (data.medal > (int)Medal.Type.None)
            {
                OnShowLevelHints?.Invoke();
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Delete Prefs")]
    void DeletePrefsAndSaves()
    {
        PlayerPrefs.DeleteAll();
    }
#endif

    void OnDestroy()
    {
        playerHealth?._OnDie.RemoveListener(EnableGameOverUI);
        OnShowLevelHints = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && NameInputController.isEnabled == false)
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Health GetPlayerHealth()
    {
        return playerHealth;
    }

    public void EnableGameOverUI()
    {
        gameOverGO.SetActive(true);
        LevelEnded = true;
    }

    public void EnableLevelCompletedUI()
    {
        levelCompletedGO.SetActive(true);
        LevelEnded = true;
    }
}
