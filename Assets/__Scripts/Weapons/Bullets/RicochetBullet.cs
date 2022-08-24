using UnityEngine;

public class RicochetBullet : Bullet
{
    [SerializeField] int maxRicochetCount;
    [SerializeField] LayerMask environmentLayers;
    int currentRicochetCount;

    protected override void OnRegisterCollision(Collider2D collision)
    {
        base.OnRegisterCollision(collision);

        if (environmentLayers.Contains(collision.gameObject.layer))
            Ricochet();
    }

    void Ricochet()
    {
        if (currentRicochetCount >= maxRicochetCount)
            Destroy(gameObject);

        var hit = Physics2D.Raycast(transform.position, transform.right, 3, environmentLayers);
        if (hit.collider == null)
            return;

        var newVelocityDir = Vector2.Reflect(rb.velocity, hit.normal);
        transform.right = newVelocityDir.normalized;
        rb.velocity = newVelocityDir;
        currentRicochetCount++;
    }
}
