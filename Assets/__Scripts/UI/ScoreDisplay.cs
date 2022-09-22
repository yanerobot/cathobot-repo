using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankTextObj, nameTextObj, timeTextObj;
    [SerializeField] Image mainImg;

    internal void SetText(int rank, string nameText, string timeText)
    {
        rankTextObj.text = rank.ToString() + ".";
        nameTextObj.text = nameText;
        timeTextObj.text = timeText;
    }

    internal void SetColor(Color color)
    {
        mainImg.color = color;
    }
}
