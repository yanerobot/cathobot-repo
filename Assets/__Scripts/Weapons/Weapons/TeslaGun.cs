using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaGun : Weapon
{
    [SerializeField] float raycastDistance;
    [SerializeField] float radius;
    [SerializeField] LayerMask targets;
    [SerializeField] LayerMask obstaclesAndTargets;
    
    protected Coroutine shooting;

    protected override void Shoot()
    {
        if (shooting != null) 
            return;

        shooting = StartCoroutine(AutoShoot());
    }
    public override void StopUsing()
    {
        if (shooting != null)
            StopCoroutine(shooting);
        shooting = null;
    }

    IEnumerator AutoShoot()
    {
        var delay = new WaitForSeconds(stats.delayBetweenShots);
        yield return null;

        while (true)
        {
            var hit = Physics2D.Raycast(shootingPoint.position, shootingPoint.up, raycastDistance);

            if (hit.collider == null)
            {
                yield return delay;
                continue;
            }

            var t = Physics2D.OverlapCircleAll(hit.transform.position, radius, targets);

            foreach (var coll in t)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.TryGetComponent(out Health health))
                {
                    health.TakeDamage(stats.damage);
                }
            }

            yield return delay;
        }
    }
}
