using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_ButtonBehaviour : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] Vector4 additionalMargin;
    [SerializeField] TextMeshProUGUI textObject;
    [SerializeField] Button button;

    Vector4 initialMargin;

    void Awake()
    {
        initialMargin = textObject.margin;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
            textObject.margin = new Vector4(0, 5.4f) + additionalMargin;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
            textObject.margin = initialMargin;
    }
}
