using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
    [SerializeField] UnityEvent onStartEvent;
    [SerializeField] UnityEvent onTriggerEnterEvent;
    [SerializeField] UnityEvent onTriggerExitEvent;
    [SerializeField] LayerMask layerMask;

    void Start()
    {
        onStartEvent?.Invoke();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (layerMask.Contains(collision.gameObject.layer))
        {
            onTriggerEnterEvent?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (layerMask.Contains(collision.gameObject.layer))
        {
            onTriggerExitEvent?.Invoke();
        }
    }
}
