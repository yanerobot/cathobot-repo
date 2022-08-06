using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBehaiv : MonoBehaviour
{
    [SerializeField] GameObject gameOverGO;
    [SerializeField] GameObject levelCompletedGO;

    Health playerHealth;
    void Awake()
    {
        gameOverGO.SetActive(false);
        var player = GameObject.FindWithTag(TopDownMovement.PLAYERTAG);

        playerHealth = player.GetComponent<Health>();
        playerHealth._OnDie.AddListener(EnableGameOverUI);
    }

    void OnDestroy()
    {
        playerHealth?._OnDie.RemoveListener(EnableGameOverUI);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
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
        EquipmentSystem.LevelEnded = true;
        TopDownMovement.LevelEnded = true;
    }

    public void EnableLevelCompletedUI()
    {
        levelCompletedGO.SetActive(true);
        EquipmentSystem.LevelEnded = true;
        TopDownMovement.LevelEnded = true;
    }

#if UNITY_EDITOR
    [ContextMenu("Delete all Prefs")]
    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
#endif
}
