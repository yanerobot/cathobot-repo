using UnityEngine;

public class SuicideCombatSystem : EnemyCombatSystem
{
    [Header("Suicide")]
    [SerializeField] protected LayerMask damagableLayers;
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionPrefab;

    public override void OnCombatStateEnter()
    {
        Attack();
    }

    public override void OnCombatStateExit()
    {

    }

    public override void OnCombatStateUpdate()
    {
        
    }

    protected override void Attack()
    {
        var hit = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damagableLayers);

        if (hit.Length > 0)
        {
            foreach (var coll in hit)
            {
                if (coll.TryGetComponent(out Health health))
                {
                    health.TakeDamage(attackDamage);
                }
            }
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
