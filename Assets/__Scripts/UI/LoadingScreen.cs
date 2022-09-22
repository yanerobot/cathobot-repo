using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] float updateEveryNSeconds;

    float currentTime;

    int currentDotsNum;
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > updateEveryNSeconds)
        {
            currentTime = 0;
            UpdateText();
        }
    }

    void UpdateText()
    {
        currentDotsNum++;

        string dots = "";

        for (int i = 0; i < currentDotsNum % 4; i++)
        {
            dots += '.';
        }

        loadingText.text = "Loading" + dots;
    }
}
