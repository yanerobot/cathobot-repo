using Pathfinding;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyAI : StateMachine
{
    [Header("References")]
    [SerializeField] public GFXBehaivior gfx;
    [SerializeField] public EnemyCombatSystem combatSystem;
    [SerializeField] LayerMask visibleLayers;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public AIDestinationSetter destinationSetter;
    [SerializeField] public AIPath aiPath;
    [SerializeField] GameObject explosionPrefab;

    [Header("Stats")]
    [SerializeField] float runningDistance;
    [SerializeField] float combatDistance;
    [SerializeField] float speedDecreaseOnHitModifier;
    [SerializeField] float stunOnDamageTime;
    [SerializeField] public bool isStatic;


    Animator animator;

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public AIManager aiManager;

    float normalSpeed;

    void Start()
    {
        normalSpeed = aiPath.maxSpeed;

        aiManager = transform.parent.GetComponent<AIManager>();

        if (isStatic)
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (destinationSetter.target != null)
            target = destinationSetter.target;

        var searching = new Searching(this);
        var running = new Running(this);
        var combat = new Combat(this);

        searching.AddTransition(running, () => IsPlayerAvailable() && IsPlayerWithinRunningRange()).AddTransitionCallBack(ActivateSounds);
        
        running.AddTransition(combat, IsPlayerWithingCombatRange, IsPlayerVisible);
        running.AddTransition(searching, () => !IsPlayerAvailable());

        combat.AddTransition(running, () => !IsPlayerWithingCombatRange() || !IsPlayerVisible());
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
        combatSystem.src.enabled = true;
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

    bool IsPlayerAvailable()
    {
        if (destinationSetter.target == null || !aiManager.canFollowPlayer)
            return false;
        return true;
    }

    public bool IsPlayerVisible()
    {
        var ray = Physics2D.CircleCast(transform.position, 1, transform.position.Direction(target.position), combatDistance, visibleLayers);

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
}
