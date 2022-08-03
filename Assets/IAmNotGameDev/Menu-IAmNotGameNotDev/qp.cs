using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class qp : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void q()
    {

        Application.Quit();
    }
}
