using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automatic : Weapon
{
    [SerializeField] float fireRateDelayChanged;
    [SerializeField] float timeToChange;

    protected Coroutine shooting;
    protected override void Shoot()
    {
        if (shooting != null) return;

        shooting = StartCoroutine(AutoShoot(stats.delayBetweenShots));
    }

    public override void StopUsing()
    {
        if (shooting != null)
            StopCoroutine(shooting);
        shooting = null;
    }

    IEnumerator AutoShoot(float delay)
    {
        float currentTime = 0;
        float finalDelay = delay;

        yield return null;

        while (true)
        {
            if (!HasBullets())
            {
                yield return null;
                continue;
            }

            SpendBullets(1);

            SingleShot();

            src.PlayOneShot(stats.shootSFX);

            if (fireRateDelayChanged != 0)
            {
                finalDelay = Mathf.Lerp(delay, fireRateDelayChanged, currentTime/timeToChange);
                currentTime += finalDelay;
            }

            yield return new WaitForSeconds(finalDelay);
        }
    }
}
