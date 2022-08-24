using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_CountDown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    void Start()
    {
        LevelStartCountDown.OnSecondPassed.AddListener(ShowNumber);
    }

    void ShowNumber(int number)
    {
        number = 3 - number;
        if (number == 0)
        {
            textMesh.text = "GO!";
            this.Co_DelayedExecute(() => Destroy(gameObject), 0.3f);
            return;
        }
        textMesh.text = number.ToString();
    }
}
