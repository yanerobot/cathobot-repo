using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject inputPanel;
    public GameObject creditsPanel;
    public AudioSource back;
    public AudioSource forward;

    bool additionalPanelOpened = false;
    private void Update()
    {
        if (!additionalPanelOpened)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            back.Play();
            creditsPanel.SetActive(false);
            inputPanel.SetActive(false);
        }
    }

    public void Play()
    {
        var lastLevel = PlayerPrefs.GetInt(ExitScript.PrefsKey, -1);

        if (lastLevel > 0 && UI_InitialGameLoader.isContinue)
        {
            SceneManager.LoadScene(lastLevel);
            return;
        }
        SceneManager.LoadScene(1);
    }


    public void EnableInputPanel()
    {
        forward.Play();
        inputPanel.SetActive(true);
        additionalPanelOpened = true;
    }


    public void EnableCreditsPanel()
    {
        forward.Play();
        creditsPanel.SetActive(true);
        additionalPanelOpened = true;
    }



    public void DisableInputPanel()
    {
        back.Play();
        inputPanel.SetActive(false);
        additionalPanelOpened = false;
    }

    public void DisableCreditsPanel()
    {
        back.Play();
        creditsPanel.SetActive(false);
        additionalPanelOpened = false;

    }
    public void Quit()
    {
        Application.Quit();
    }

}
