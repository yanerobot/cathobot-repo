using Pathfinding;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyAI : StateMachine
{
    [Header("References")]
    [SerializeField] GFXBehaivior gfx;
    [SerializeField] public Transform staticTurret;
    [SerializeField] LayerMask visibleLayers;
    [SerializeField] LayerMask damagableLayers;
    [SerializeField] Transform hitPoint;
    [SerializeField] Transform hitPoint2;
    public Rigidbody2D rb;
    public AIDestinationSetter destinationSetter;
    public AIPath aiPath;
    public AIManager aiManager;
    [SerializeField] GameObject explosionPrefab;

    Animator animator;

    [HideInInspector]
    public Transform target;
    [Header("Stats")]
    [SerializeField] int attackDamage;
    [SerializeField] float attackRadius;
    [SerializeField] float runningDistance;
    [SerializeField] float combatDistance;
    [SerializeField] float speedDecreaseOnHitModifier;
    [SerializeField] float stunOnDamageTime;
    [SerializeField] public float delayBetweenAttacks;
    [SerializeField] public bool isRanged, isStatic, isExplosive;
    [SerializeField] float explosionRadius;

    [Header("Bullet")]
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;
    [SerializeField] GameObject bulletPrefab;

    [Header("Sounds")]
    [SerializeField, Tooltip("If false then sound plays on combat mode")] public bool isSoundOnAttack;
    [SerializeField] public AudioClip attackAudioClip;
    [SerializeField] public AudioSource src;

    public Vector2 initialPoint;
    float normalSpeed;

    void Start()
    {
        normalSpeed = aiPath.maxSpeed;
        initialPoint = transform.position;

        if (aiManager == null)
            aiManager = transform.parent.GetComponent<AIManager>();

        if (isStatic)
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (destinationSetter.target != null)
            target = destinationSetter.target;

        if (src != null)
            src.enabled = false;

        var searching = new Searching(this);
        var running = new Running(this);
        var combat = new Combat(this);

        searching.AddTransition(running, () => IsPlayerAvailable() && IsPlayerWithinRunningRange()).AddTransitionCallBack(ActivateSounds);
        
        running.AddTransition(combat, () => IsPlayerWithingCombatRange() && IsPlayerVisible());
        running.AddTransition(searching, () => !IsPlayerAvailable());

        combat.AddTransition(running, () => !IsPlayerWithingCombatRange());
        combat.AddTransition(searching, IsTargetDead);

        SetState(searching);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Bullet bullet) || isStatic)
            return;

        aiPath.canMove = false;
        Invoke(nameof(EnableMovement), stunOnDamageTime);
    }

    void ActivateSounds()
    {
        src.enabled = true;
    }

    public void SetTarget(Transform target)
    {
        destinationSetter.target = target;
        this.target = target;
    }

    public bool IsTargetDead()
    {
        return aiManager.target.isDead;
    }

    public void PlayHitAnimation()
    {
        //print("Displaying 'GetHit' animation");
    }

    public bool Attack()
    {
        if (isRanged)
            return RangedAttack();
        else if (isExplosive)
            Explode();
        else
            MeleeAttack();
        return true;
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (Selection.activeGameObject != transform.gameObject)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, runningDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, combatDistance);
    }
#endif
    void Explode()
    {
        var hit = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damagableLayers);

        if (hit.Length > 0)
        {
            foreach(var coll in hit)
            {
                if (coll.TryGetComponent(out Health health)){
                    health.TakeDamage(attackDamage);
                }
            }
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void MeleeAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(hitPoint.position, attackRadius, damagableLayers);

        foreach (var coll in colliders)
        {
            if (coll.TryGetComponent(out Health health))
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    Transform prevHitPoint;
    bool RangedAttack()
    {
        if (!IsPlayerVisible())
            return false;

        var shootPoint = hitPoint;

        if (hitPoint2 != null && prevHitPoint == hitPoint)
        {
            shootPoint = hitPoint2;
            prevHitPoint = hitPoint2;
        }
        else
        {
            prevHitPoint = hitPoint;
        }


        var go = Instantiate(bulletPrefab, shootPoint.position, hitPoint.rotation, null);
        var bullet = go.GetComponent<Bullet>();

        bullet.Init(gameObject, bulletDamage, bulletSpeed);

        return true;
    }

    bool IsPlayerAvailable()
    {
        if (destinationSetter.target == null || !aiManager.canFollowPlayer)
            return false;
        return true;
    }

    bool IsPlayerVisible()
    {
        if (!isRanged)
            return true;

        var ray = Physics2D.Raycast(transform.position, transform.Direction(target.position), combatDistance, visibleLayers);

        if (ray.collider != null && ray.collider.transform == target)
            return true;
        return false;
    }

    public bool IsPlayerWithinRunningRange()
    {
        return aiPath.remainingDistance < runningDistance;
    }

    bool IsPlayerWithingCombatRange()
    {
        if (target == null)
            return false;

        return Vector3.Distance(transform.position, target.position) <= combatDistance;
    }

    public void OnDamage()
    {
        var time = gfx.Blink();

        aiPath.maxSpeed *= speedDecreaseOnHitModifier;
        this.Co_DelayedExecute(() => aiPath.maxSpeed = normalSpeed, time);
    }

    void EnableMovement()
    {
        aiPath.canMove = true;
        rb.velocity *= 0;
        rb.angularVelocity *= 0;
    }

    public void OnDie()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
