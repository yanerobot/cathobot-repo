using UnityEngine;

public class MeleeCombatSystem : EnemyCombatSystem
{
    [Header("Melee")]
    [SerializeField] protected LayerMask damagableLayers;
    [SerializeField] protected float attackRadius;
    protected override void Attack()
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
}
