using System;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class SafeZone : MonoBehaviour
{
    public const string TAG = "SafeZone";
    
    public static UnityEvent OnSafeZoneOut;

    void OnEnable()
    {
        OnSafeZoneOut = new UnityEvent();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TopDownMovement.PLAYERTAG)
        {
            OnSafeZoneOut?.Invoke();
            Destroy(gameObject);
        }
    }

    void OnDisable()
    {
        OnSafeZoneOut?.RemoveAllListeners();
    }
}
