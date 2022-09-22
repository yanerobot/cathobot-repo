using UnityEngine;
using UnityEngine.SceneManagement;
public class RP : MonoBehaviour
{
    [SerializeField] GameObject OnlyValuableBuffsObj;
    public AudioSource back;
    public AudioSource forward;

    public static bool GameIsPaused = false;
    public GameObject PauseUI;

    void OnEnable()
    {
        OnlyValuableBuffsObj.SetActive(false);
        if (PlayerPrefs.GetInt(OnlyValuableBuffsToggle.EnabledPrefsKey, 0) == 1)
        {
            OnlyValuableBuffsObj.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (GameIsPaused)
            {
                forward.Play();
                Resume();
            }
            else
            {
                back.Play();
                Pause();
            }
        }
    }


    public void Resume()

    {
        GameIsPaused = false;
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }


    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
        PauseUI.SetActive(true);
    }


    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void Quit()
    {
       Application.Quit();
    }



}
