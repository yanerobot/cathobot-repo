using UnityEngine;

public class RangedCombatSystem : EnemyCombatSystem
{
    [Header("Ranged")]
    [SerializeField] Transform[] hitPoints;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float bulletSpeed;

    int currentHitPointIndex;

    TimeCondition changeDirDelay;

    Vector2 currentOffset;
    float currentLerp;

    public override void OnCombatStateEnter()
    {
        base.OnCombatStateEnter();
        changeDirDelay = new TimeCondition(0.5f);
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

/*    
 *    Prediction Shooting:
 *    protected override Vector2 GetDirection()
    {
        var dir = base.GetDirection();

        if (playerMovement != null && changeDirDelay.HasTimePassed())
        {
            currentLerp = 0;
            changeDirDelay.ResetTimer();
            var dist = Vector2.Distance(AI.transform.position, AI.target.position);
            var time = dist / bulletSpeed;

            if (dist > 2 && Vector2.Dot(dir, playerMovement.MovementVector) > -0.2f)
            {
                currentOffset = playerMovement.MovementVector / (dist / 2);
            }
        }
        currentLerp += 0.2f;
        return Vector2.Lerp(dir, dir + currentOffset, currentLerp);
    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere((Vector2)transform.position + currentOffset, 0.2f);
    }
}
