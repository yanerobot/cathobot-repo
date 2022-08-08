using UnityEngine;
using System.Collections.Generic;

public class PoisonPuddle : MonoBehaviour
{
    [SerializeField] Color poisonedColor;
    [SerializeField] int damagePerTick;
    [SerializeField] float tickRate;
    [SerializeField] float timeToTickAfterLeaving;

    Transform lowerDamageCharacter;
    int lowerDamage;

    const string poisonTickDamageKey = "poison";

    public void DestroyAfterTime(float time)
    {
        this.Co_DelayedExecute(() => Destroy(gameObject), time);
    }

    public void SetDamage(int damage)
    {
        damagePerTick = damage;
    }

    public void SetDamageToChar(Transform transform, int damage)
    {
        lowerDamageCharacter = transform;
        lowerDamage = damage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            if (collision.transform == lowerDamageCharacter)
                health.TickDamage(poisonTickDamageKey, tickRate, lowerDamage);
            else
                health.TickDamage(poisonTickDamageKey, tickRate, damagePerTick);

            health.GetComponentInChildren<CharacterGFXBehaivior>()?.TiltTickColor(poisonedColor);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            health.StopTickDamage(poisonTickDamageKey, timeToTickAfterLeaving, () => health.GetComponentInChildren<CharacterGFXBehaivior>()?.ResetDamagedColor());
        }
    }
}
