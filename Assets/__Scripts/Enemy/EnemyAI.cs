using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyAI : StateMachine, IStunnable
{
    [Header("References")]
    [SerializeField] public CharacterGFXBehaivior gfx;
    [SerializeField] public EnemyCombatSystem combatSystem;
    [SerializeField] public LayerMask visibleLayers;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public AIDestinationSetter destinationSetter;
    [SerializeField] public AIPath aiPath;
    [SerializeField] GameObject explosionPrefab;

    [Header("Stats")]
    [SerializeField, Tooltip("If unchecked, distance check will be calculated by AI path remaining distance")] bool checkByDistance;
    [SerializeField] float runningDistance;
    [SerializeField] float runningCombatDistance;
    [SerializeField] float combatDistance;
    [SerializeField] float speedDecreaseOnHitModifier;
    [SerializeField] float stunOnDamageTime;
    [SerializeField] public bool isStatic;

    [HideInInspector]
    public bool movementDisabledExternally;

    Animator animator;

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public AIManager aiManager;

    float normalSpeed;

    public UnityAction OnRunningEnter, OnRunningUpdate, OnRunningExit;

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
        var runningCombat = new RunningCombat(this);
        var combat = new Combat(this);

        searching.AddTransition(running, () => IsPlayerAvailable() && IsPlayerWithinRunningRange()).AddTransitionCallBack(ActivateSounds);

        running.AddTransition(combat, IsPlayerWithingCombatRange, IsPlayerVisible);
        running.AddTransition(searching, () => !IsPlayerAvailable());
        running.AddTransition(runningCombat, IsPlayerWithingRunningCombatRange, () => !IsPlayerWithingCombatRange(), IsPlayerVisible);

        combat.AddTransition(running, () => !IsPlayerWithingRunningCombatRange() || !IsPlayerVisible());
        combat.AddTransition(runningCombat, () => !IsPlayerWithingCombatRange(), IsPlayerWithingRunningCombatRange);
        combat.AddTransition(searching, IsTargetDead);

        runningCombat.AddTransition(running, () => !IsPlayerWithingRunningCombatRange() || !IsPlayerVisible());
        runningCombat.AddTransition(combat, IsPlayerWithingCombatRange, IsPlayerVisible);

        SetState(searching);
    }

    void ActivateSounds()
    {
        combatSystem.src.enabled = true;
    }

    public void ResetRigidbody()
    {
        rb.velocity *= 0;
        rb.angularVelocity *= 0;
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

    public bool IsPlayerAvailable()
    {
        if (destinationSetter.target == null || !aiManager.canFollowPlayer)
            return false;
        return true;
    }

    public bool IsPlayerVisible()
    {
        var ray = Physics2D.Raycast(transform.position, transform.position.Direction(target.position), runningCombatDistance, visibleLayers);

        if (ray.collider != null && ray.collider.transform == target)
            return true;
        return false;
    }

    public bool IsPlayerWithinRunningRange()
    {
        if (target == null)
            return false;
        float distance = aiPath.remainingDistance;
        if (checkByDistance)
            distance = Vector2.Distance(transform.position, target.position);
        return distance < runningDistance;
    }

    bool IsPlayerWithingCombatRange()
    {
        if (target == null)
            return false;

        return Vector3.Distance(transform.position, target.position) <= combatDistance;
    }
    bool IsPlayerWithingRunningCombatRange()
    {
        if (target == null)
            return false;

        return aiPath.remainingDistance <= runningCombatDistance;
    }

    public void OnDamage()
    {
        var time = gfx.Blink();

        aiPath.maxSpeed *= speedDecreaseOnHitModifier;
        this.Co_DelayedExecute(() => aiPath.maxSpeed = normalSpeed, time);
    }
    public void DisableMovement(float time = 0)
    {
        aiPath.canMove = false;
        movementDisabledExternally = true;
        if (time > 0)
            Invoke(nameof(EnableMovement), time);
    }

    public void EnableMovement()
    {
        movementDisabledExternally = false;
        aiPath.canMove = true;
        ResetRigidbody();
    }

    public void BuffSpeed(float modifier, float time)
    {
        aiPath.maxSpeed *= modifier;
        this.Co_DelayedExecute(() => aiPath.maxSpeed /= modifier, time);
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runningCombatDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, combatDistance);
    }
#endif
}
