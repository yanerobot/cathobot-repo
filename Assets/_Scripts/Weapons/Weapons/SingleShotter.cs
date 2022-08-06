using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotter : Weapon
{
    protected override void Shoot()
    {
        if (CurrentBullets <= 0)
            return;

        CurrentBullets--;

        SingleShot();
    }
}
