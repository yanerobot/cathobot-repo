using UnityEngine;
using System.Collections;
public class ExplosiveBullet : Bullet
{
    [SerializeField] GameObject explosionObject;
    [SerializeField] float explosionRadius, explosionForce;
    [SerializeField] LayerMask explosiveLayers;
    
    protected override bool RegisterHit(Collider2D _)
    {
        var collisions = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosiveLayers);

        foreach (var coll in collisions)
        {
            if (coll.gameObject == holder.gameObject)
                continue;

            if (coll.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }

            Push(coll.attachedRigidbody);
        }


        Instantiate(explosionObject, transform.position, Quaternion.identity);

        return true;
    }

    public void Push(Rigidbody2D rbToPush)
    {
        if (rbToPush == null)
            return;

        Vector2 dir = (rbToPush.position - rb.position).normalized;
        float distanceModifier = Mathf.Clamp(1 / Vector2.Distance(rb.position, rbToPush.position), 0.3f, 1);

        rbToPush.AddForce(dir * explosionForce * distanceModifier, ForceMode2D.Impulse);
    }
}
