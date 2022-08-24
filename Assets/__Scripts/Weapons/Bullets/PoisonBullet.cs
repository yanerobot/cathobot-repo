using UnityEngine;

public class PoisonBullet : Bullet
{
    [SerializeField] PoisonPuddle poisonPuddle;
    [SerializeField] float poisonPuddleTime;
    [SerializeField] int damageToHolder;

    void Update()
    {
        if (rb.velocity.magnitude < 1.5f)
        {
            SpillPoison();
        }
    }

    protected override void OnRegisterCollision(Collider2D collision)
    {
        SpillPoison();
    }

    void SpillPoison()
    {
        var puddle = Instantiate(poisonPuddle, transform.position, transform.rotation);
        puddle.DestroyAfterTime(poisonPuddleTime);
        puddle.SetDamage(damage);
        puddle.SetDamageToChar(holder.transform, damageToHolder);
        Destroy(gameObject);
    }
}
