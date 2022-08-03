using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;
    public AudioSource back;
    public AudioSource forward;

    bool canvas = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if(canvas == true) {
            back.Play();
            canvas2.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas == true)
            {
                back.Play();
                canvas1.SetActive(false);
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void ControlM()
    {
        forward.Play();
        canvas1.SetActive(true);
        canvas = true;
    }


    public void Credits()
    {
        forward.Play();
        canvas2.SetActive(true);
        canvas = true;
    }



    public void disablecanvas1()
    {
        back.Play();
        canvas1.SetActive(false);
        canvas = false;

    }

    public void disablecanvas2()
    {
        back.Play();
        canvas2.SetActive(false);
        canvas = false;

    }
    public void Quit()
    {
        Application.Quit();
    }

}
