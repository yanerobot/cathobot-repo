using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_ButtonBehaviour : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] TextMeshProUGUI textObject;

    public void OnPointerDown(PointerEventData eventData)
    {
        textObject.margin = new Vector4(0, 5.4f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        textObject.margin = new Vector4(0, 0);
    }
}
