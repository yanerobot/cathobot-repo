using UnityEngine;

public class PoisonBullet : Bullet
{
    [SerializeField] GameObject poisonPuddle;
    [SerializeField] float poisonPuddleTime;

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
        var go = Instantiate(poisonPuddle, transform.position, transform.rotation);
        var puddle = go.GetComponent<PoisonPuddle>();
        puddle.DestroyAfterTime(poisonPuddleTime);
        puddle.SetDamage(damage);
        Destroy(gameObject);
    }
}
