using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RP : MonoBehaviour
{
    public AudioSource back;
    public AudioSource forward;

    public static bool GameIsPaused = false;
    public GameObject PauseUI;

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
