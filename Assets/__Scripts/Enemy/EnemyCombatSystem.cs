using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public abstract class EnemyCombatSystem : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected Transform hitPoint;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float delayBetweenAttacks;
    [SerializeField] protected AudioClip attackingAudio;
    [SerializeField] protected bool isSoundOnAttack;

    [SerializeField] float preparationStateTime;
    [SerializeField] float attackingStateTime;

    [HideInInspector]
    public AudioSource src;

    TimeCondition attackDelay, preparationTC;

    AudioClip mainClip;

    protected EnemyAI AI;

    protected TopDownMovement playerMovement;

    bool hasPreparationStarted;
    protected bool hasPreparationFinished;

    void Start()
    {
        src = GetComponent<AudioSource>();
        AI = GetComponent<EnemyAI>();
    }

    public virtual void OnCombatStateEnter()
    {
        if (attackDelay == null)
        {
            attackDelay = new TimeCondition(delayBetweenAttacks);
            attackDelay.ReduceTime(delayBetweenAttacks);
        }

        preparationTC = new TimeCondition(preparationStateTime);

        if (!isSoundOnAttack)
        {
            if (attackingAudio != null)
            {
                mainClip = src.clip;
                src.clip = attackingAudio;
                src.Play();
            }
        }

        attackingStateTC = new TimeCondition(attackingStateTime);

        playerMovement = AI.target.GetComponent<TopDownMovement>();
    }

    TimeCondition attackingStateTC;

    public virtual void OnCombatStateUpdate()
    {
        LookAtTarget();

        if (PrepareAttack())
            return;

        if (attackingStateTC.HasTimePassed() && attackingStateTime > 0)
        {
            preparationTC.ResetTimer();
        }

        if (attackDelay.HasTimePassed())
        {
            Attack();
            attackDelay.ResetTimer();

            if (isSoundOnAttack)
                src.PlayOneShot(attackingAudio);
        }
    }

    void LookAtTarget()
    {
        var dir = GetDirection(); 

        if (AI.isStatic)
            AI.gfx.transform.up = dir;
        else
            AI.transform.up = dir;
        
    }

    protected virtual Vector2 GetDirection()
    {
        return (Vector2)AI.target.position - (Vector2)AI.transform.position;
    }

    public virtual void OnCombatStateExit()
    {
        if (mainClip != null)
        {
            src.clip = mainClip;
            src.Play();
        }
        hasPreparationStarted = false;
    }

    bool PrepareAttack()
    {
        if (preparationStateTime <= 0)
            return false;

        if (!preparationTC.HasTimePassed())
        {
            hasPreparationFinished = false;
            if (!hasPreparationStarted)
            {
                OnPrepareAttackStart();
                hasPreparationStarted = true;
            }
            OnPrepareAttackUpdate();
        }
        else if (!hasPreparationFinished)
        {
            OnPrepareAttackEnd();
            hasPreparationFinished = true;
            hasPreparationStarted = false;
            attackingStateTC.ResetTimer();
        }
        return !hasPreparationFinished;
    }

    protected abstract void Attack();

    protected virtual void OnPrepareAttackStart() { }
    protected virtual void OnPrepareAttackUpdate() { }
    protected virtual void OnPrepareAttackEnd() { }

    public virtual bool CanExit() { return true; }
}
