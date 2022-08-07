using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [SerializeField] GameObject explosionObject;
    [SerializeField] float explosionRadius, explosionForce;
    [SerializeField] LayerMask explosiveLayers;
    
    protected override void OnRegisterCollision(Collider2D _)
    {
        Explode();
    }

    public void Explode()
    {
        var collisions = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosiveLayers);

        foreach (var coll in collisions)
        {
            if (coll.gameObject != holder.gameObject &&  coll.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }

            Push(coll.attachedRigidbody);
        }

        Instantiate(explosionObject, transform.position, Quaternion.identity);
    }

    void Push(Rigidbody2D rbToPush)
    {
        if (rbToPush == null)
            return;

        Vector2 dir = (rbToPush.position - rb.position).normalized;
        float distanceModifier = Mathf.Clamp(1 / Vector2.Distance(rb.position, rbToPush.position), 0.3f, 1);

        if (rbToPush.TryGetComponent(out IStunnable stunnable))
        {
            stunnable.Stun(0.5f);
        }

        rbToPush.AddForce(dir * explosionForce * distanceModifier, ForceMode2D.Impulse);
    }
}
