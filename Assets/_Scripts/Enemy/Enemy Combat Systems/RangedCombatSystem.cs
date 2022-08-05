using UnityEngine;

public class RangedCombatSystem : EnemyCombatSystem
{
    [Header("Ranged")]
    [SerializeField] Transform[] hitPoints;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float bulletSpeed;

    int currentHitPointIndex;

    public override void OnCombatStateEnter()
    {
        base.OnCombatStateEnter();
        currentHitPointIndex = 0;
    }

    protected override void Attack()
    {
        var shootPoint = hitPoints[currentHitPointIndex];

        currentHitPointIndex = (currentHitPointIndex + 1) % hitPoints.Length;


        var go = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation, null);
        var bullet = go.GetComponent<Bullet>();

        bullet.Init(gameObject, attackDamage, bulletSpeed);
    }
}
