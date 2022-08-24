using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected AudioSource audioSrc;
    [SerializeField] bool destroyOnCollision;
    [SerializeField] float destroyselfAfter = 6;

    protected GameObject holder;

    protected int damage = 10;
    protected float speed = 30f;

    bool collided;

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Init(GameObject holder, int damage, float speed, float modifier = 1)
    {
        this.damage = Mathf.RoundToInt(damage * modifier);
        this.speed = speed;
        this.holder = holder; 
        
        rb.velocity = transform.right * speed;
        Invoke(nameof(DestroySelf), destroyselfAfter);
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
            health.TakeDamage(Mathf.RoundToInt(damage));
        }

    }
}