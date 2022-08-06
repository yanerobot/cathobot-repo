using UnityEngine;

public class PoisonPuddle : MonoBehaviour
{
    [SerializeField] Color poisonedColor;
    [SerializeField] int damagePerTick;
    [SerializeField] float tickRate;
    [SerializeField] float timeToTickAfterLeaving;

    public void DestroyAfterTime(float time)
    {
        this.Co_DelayedExecute(() => Destroy(gameObject), time);
    }

    public void SetDamage(int damage)
    {
        damagePerTick = damage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            health.TickDamage(tickRate, damagePerTick, transform.GetInstanceID());
            health.GetComponentInChildren<CharacterGFXBehaivior>()?.TiltTickColor(poisonedColor);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            this.Co_DelayedExecute(() => Unpoison(health), timeToTickAfterLeaving);
        }
    }

    void Unpoison(Health health)
    {
        if (health == null)
            return;

        health.StopTickDamage(transform.GetInstanceID());
        health.GetComponentInChildren<CharacterGFXBehaivior>()?.ResetDamagedColor();
    }
}
