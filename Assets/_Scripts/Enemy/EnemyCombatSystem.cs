using System.Collections;
using UnityEngine;

public abstract class EnemyCombatSystem : MonoBehaviour
{
    [SerializeField] protected EnemyAI AI;

    public abstract void OnCombatStateEnter();

    public abstract void OnCombatStateUpdate();

    public abstract void OnCombatStateExit();

    protected IEnumerator StartAttacking()
    {
        yield return new WaitForSeconds(AI.delayBetweenAttacks / 2);
        while (true)
        {
            if (AI.Attack())
                if (AI.isSoundOnAttack)
                    AI.src.PlayOneShot(AI.attackAudioClip);

            yield return new WaitForSeconds(AI.delayBetweenAttacks);
        }
    }
/*    public bool Attack()
    {
        if (isRanged)
            return RangedAttack();
        else if (isExplosive)
            Explode();
        else
            MeleeAttack();
        return true;
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
    }*/
}
