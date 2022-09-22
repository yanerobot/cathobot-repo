using UnityEngine;
using UnityEngine.Events;

public class HitboxBuffBehavior : MonoBehaviour
{
    [SerializeField] UnityEvent OnHitboxBuffStart;
    [SerializeField] UnityEvent OnHitboxBuffEnd;

    void Awake()
    {
        Hitbox.OnHitboxBuffStart += OnBuffStart;
        Hitbox.OnHitboxBuffEnd += OnBuffEnd;
    }

    void OnDestroy()
    {
        Hitbox.OnHitboxBuffStart -= OnBuffStart;
        Hitbox.OnHitboxBuffEnd -= OnBuffEnd;
    }

    void OnBuffStart()
    {
        OnHitboxBuffStart?.Invoke();
    }

    void OnBuffEnd()
    {
        OnHitboxBuffEnd?.Invoke();
    }
}
