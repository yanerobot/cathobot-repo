using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GOonEnable : MonoBehaviour
{

    void OnEnable()
    {
        var dice = FindObjectOfType<Dice>();
        if (dice != null)
            dice.StopDice();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
