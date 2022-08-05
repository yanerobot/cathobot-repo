using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected AudioSource audioSrc;

    protected GameObject holder;

    protected int damage = 10;
    protected float speed = 30f;

    float modifier;

    bool collided;

    public bool Pierce;

    void Start()
    {
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
        Invoke(nameof(DestroySelf), 6);
    }

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

        if (!Pierce)
        {
            collided = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            transform.SetParent(collision.transform);
        }

        bool isHitHealth = RegisterHit(collision);

        if (Pierce)
            return;

        if (!Pierce || !isHitHealth)
            Destroy(gameObject);
    }

    protected virtual bool RegisterHit(Collider2D collision)
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

        return health != null;
    }
}
