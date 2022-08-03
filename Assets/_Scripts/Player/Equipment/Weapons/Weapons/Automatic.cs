using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automatic : Weapon
{
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
        while (true)
        {
            SingleShot();
            src.PlayOneShot(stats.shootSFX);
            yield return new WaitForSeconds(delay);
        }
    }
}
