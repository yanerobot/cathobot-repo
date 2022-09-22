using Pathfinding;
using UnityEngine;

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
    [SerializeField] float visibilityRange;
    [SerializeField] float speedDecreaseOnHitModifier;
    [SerializeField] float stunOnDamageTime;
    [SerializeField] public bool isStatic;

    [Header("Special Attack")]
    [SerializeField] public SpecialAbilityData[] specialAbilities;
    

    [HideInInspector]
    public bool movementDisabledExternally;

    Animator animator;

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public AIManager aiManager;

    float normalSpeed;
    bool sawPlayer;

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

        running.AddTransition(combat, IsPlayerWithinCombatRange, IsPlayerVisible);
        running.AddTransition(searching, () => !IsPlayerAvailable());
        running.AddTransition(runningCombat, IsPlayerWithinRunningCombatRange, () => !IsPlayerWithinCombatRange(), IsPlayerVisible);

        combat.AddTransition(running, () => !IsPlayerWithinRunningCombatRange() || !IsPlayerVisible(), combatSystem.CanExit);
        combat.AddTransition(searching, () => IsTargetDead() || !IsPlayerWithinRunningRange());
        combat.AddTransition(runningCombat, () => !IsPlayerWithinCombatRange(), IsPlayerWithinRunningCombatRange, combatSystem.CanExit);

        runningCombat.AddTransition(running, () => !IsPlayerWithinRunningCombatRange() || !IsPlayerVisible(), combatSystem.CanExit);
        runningCombat.AddTransition(combat, IsPlayerWithinCombatRange, IsPlayerVisible, combatSystem.CanExit);


        if (specialAbilities.Length > 0)
        {
            foreach (var sa in specialAbilities)
            {
                var special = new SpecialAttack(this, sa.component);
                if (sa.combat)
                {
                    combat.AddTransition(special, sa.component.EnterCondition);
                }
                if (sa.running)
                {
                    running.AddTransition(special, sa.component.EnterCondition);
                }
                if (sa.runningCombat)
                {
                    runningCombat.AddTransition(special, sa.component.EnterCondition);
                }
                special.AddTransition(combat, IsPlayerWithinCombatRange, IsPlayerVisible, sa.component.ExitCondition);
                special.AddTransition(runningCombat, IsPlayerWithinRunningCombatRange, () => !IsPlayerWithinCombatRange(), IsPlayerVisible, sa.component.ExitCondition);
                special.AddTransition(running, () => !IsPlayerWithinRunningCombatRange() || !IsPlayerVisible(), sa.component.ExitCondition);
                sa.component.InitiateConditions();
            }
        }

        AddAnyTransition(searching, () => !IsPlayerAvailable()).Except(searching);

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
        if (target == null)
            return false;

        var ray = Physics2D.Raycast(transform.position, transform.position.Direction(target.position), visibilityRange, visibleLayers);

        if (ray.collider != null && ray.collider.transform == target)
        {
            if (!sawPlayer)
            {
                sawPlayer = true;
                foreach (var abil in specialAbilities)
                {
                    abil.component.ResetConditions();
                }
            }
            return true;
        }
        return false;
    }

    public bool IsPlayerWithinRunningRange()
    {
        if (target == null)
            return false;
        float distance = GetRunningDistance();
        return distance < runningDistance;
    }

    public float GetRunningDistance()
    {
        if (checkByDistance)
            return Vector2.Distance(transform.position, target.position);

        return aiPath.remainingDistance;
    }

    bool IsPlayerWithinCombatRange()
    {
        if (target == null)
            return false;

        return Vector3.Distance(transform.position, target.position) <= combatDistance;
    }
    bool IsPlayerWithinRunningCombatRange()
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
