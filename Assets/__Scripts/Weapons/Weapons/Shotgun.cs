using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] AudioClip shotGunReload;
    ShotgunStatsSO shotgunStats;

    public override void WasEquippedBy(EquipmentSystem character)
    {
        base.WasEquippedBy(character);
        shotgunStats = stats as ShotgunStatsSO;
    }

    protected override void Shoot()
    {
        if (canShoot == false)
            return;
     
        if (!HasBullets())
            return;

        SpendBullets(1);

        canShoot = false;
        Invoke(nameof(EnableShooting), stats.delayBetweenShots);

        if (shotGunReload != null)
            src.PlayOneShot(shotGunReload);

        SpreadShot();
    }

    void SpreadShot()
    {
        float singleBulletAngle = shotgunStats.shotgunSpreadAngle / shotgunStats.shotgunMaxBulletsPershot;

        for (int i = 0; i < shotgunStats.shotgunMaxBulletsPershot; i++)
        {
            float angle = (i - Mathf.FloorToInt(shotgunStats.shotgunMaxBulletsPershot * 0.5f)) * singleBulletAngle;
            CreateBullet(angle);
            PlayShootEffects();
        }
    }
}
