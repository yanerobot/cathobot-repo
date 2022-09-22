using UnityEngine;
using UnityEngine.EventSystems;

public class UI_HoverMessage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RectTransform message;
    [SerializeField] Vector2 offsetFromPointer;

    public bool isEnabled;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEnabled)
            return;

        this.Co_DelayedExecute(() =>
        {
            message.transform.position = eventData.position + offsetFromPointer;
            message.gameObject.SetActive(true);
        }, 0.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        message.gameObject.SetActive(false);
    }
}
