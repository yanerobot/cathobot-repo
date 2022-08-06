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

    [HideInInspector]
    public AudioSource src;

    TimeCondition attackDelay;

    AudioClip mainClip;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public virtual void OnCombatStateEnter()
    {
        if (attackDelay == null)
            attackDelay = new TimeCondition(delayBetweenAttacks);

/*        if (attackDelay.HasTimePassed())
            attackDelay.ResetTimer();*/
        
        if (!isSoundOnAttack)
        {
            if (attackingAudio != null)
            {
                mainClip = src.clip;
                src.clip = attackingAudio;
                src.Play();
            }
        }
    }

    public virtual void OnCombatStateUpdate()
    {
        if (attackDelay.HasTimePassed())
        {
            Attack();
            attackDelay.ResetTimer();


            if (isSoundOnAttack)
                src.PlayOneShot(attackingAudio);
        }
    }

    public virtual void OnCombatStateExit()
    {
        if (mainClip != null)
        {
            src.clip = mainClip;
            src.Play();
        }
    }

    protected abstract void Attack();
}
