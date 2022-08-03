using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBehaiv : MonoBehaviour
{
    [SerializeField] GameObject gos;
    [SerializeField] GameObject egs;

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

    public void EnableGameOverUI()
    {
        gos.SetActive(true);
        EquipmentSystem.LevelEnded = true;
        TopDownMovement.LevelEnded = true;
    }

    public void EnableLevelCompletedUI()
    {
        egs.SetActive(true);
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
