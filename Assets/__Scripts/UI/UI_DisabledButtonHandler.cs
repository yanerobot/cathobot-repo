using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DisabledButtonHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textObject;
    [SerializeField] Button button;
    [SerializeField] Color disabledTextColor;
    [SerializeField] Color disabledColorTint;
    [SerializeField] UI_HoverMessage disabledHint;

    bool changedColors;
    Color normalTextColor, normalButtonColor;
    void Awake()
    {
        normalButtonColor = button.image.color;
        normalTextColor = textObject.color;
    }
    void Update()
    {
        disabledHint.isEnabled = !button.interactable;
        if (button.interactable)
        {
            button.image.color = normalButtonColor;
            textObject.color = normalTextColor;
            changedColors = false;
            return;
        }
        if (changedColors)
        {
            return;
        }

        changedColors = true;
        button.image.color *= disabledColorTint;
        textObject.color = disabledTextColor;
    }
}
