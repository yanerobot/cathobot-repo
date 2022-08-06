using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected AudioSource audioSrc;
    [SerializeField] bool destroyOnCollision;

    protected GameObject holder;

    protected int damage = 10;
    protected float speed = 30f;

    float modifier;

    bool collided;

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Init(GameObject holder, int damage, float speed, float modifier = 1)
    {
        this.modifier = modifier;
        this.damage = damage;
        this.speed = speed;
        this.holder = holder; 
        
        rb.velocity = transform.right * speed;
        Invoke(nameof(DestroySelf), 6);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (holder == null)
        {
            Destroy(gameObject);
            return;
        }

        if (collision.isTrigger || collided)
            return;

        if (collision.gameObject == holder || collision.gameObject.layer == holder.layer)
            return;

        if (destroyOnCollision)
        {
            collided = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            transform.SetParent(collision.transform);
        }

        OnRegisterCollision(collision);

        if (!destroyOnCollision)
            return;

        Destroy(gameObject);
    }

    protected virtual void OnRegisterCollision(Collider2D collision)
    {
        SimpleDamage(collision);
    }

    protected void SimpleDamage(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D otherRB))
        {
            if (!otherRB.isKinematic)
                otherRB.AddForce(transform.right * (speed / 5), ForceMode2D.Impulse);
        }

        if (collision.TryGetComponent(out Health health))
        {
            health.TakeDamage(Mathf.RoundToInt(damage * modifier));
        }

    }
}
