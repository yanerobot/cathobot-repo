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

        CurrentBullets--;

        canShoot = false;
        Invoke(nameof(EnableShooting), stats.delayBetweenShots);

        if (shotGunReload != null)
            src.PlayOneShot(shotGunReload);

        SpreadShot();
    }

    void SpreadShot()
    {

        GameObject bullet;

        float singleBulletAngle = shotgunStats.shotgunSpreadAngle / shotgunStats.shotgunMaxBulletsPershot;

        for (int i = 0; i < shotgunStats.shotgunMaxBulletsPershot; i++)
        {
            bullet = Instantiate(stats.bulletPrefab, shootingPoint.position, shootingPoint.rotation, null);
            bullet.transform.Rotate(Vector3.forward, (i - Mathf.FloorToInt(shotgunStats.shotgunMaxBulletsPershot * 0.5f)) * singleBulletAngle);

            bullet.GetComponent<Bullet>().Init(character.gameObject, stats.damage, stats.bulletSpeed, character.Modifier);

            PlayShootEffects();
        }
    }
}
