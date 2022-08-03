using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : Item
{
    [Header("Weapon")]
    [SerializeField] protected WeaponStatsSO stats;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] ParticleSystem shootEffect;

    protected AudioSource src;
    SpriteRenderer GFX;

    protected bool canShoot = true;

    protected override void Awake()
    {
        base.Awake();
        GFX = GetComponentInChildren<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    protected virtual void Shoot()
    {
        SingleShot();
    }

    protected void SingleShot()
    {
        if (canShoot == false && !(this is Automatic))
            return;

        canShoot = false;
        Invoke(nameof(EnableShooting), stats.delayBetweenShots);

        var bulletGO = Instantiate(stats.bulletPrefab, shootingPoint.position, shootingPoint.rotation, null);
        var bullet = bulletGO.GetComponent<Bullet>();

        bullet.Init(character.gameObject, stats.damage, stats.bulletSpeed, character.Modifier);

        PlayShootEffects();
    }

    protected void EnableShooting()
    {
        canShoot = true;
    }

    protected virtual void PlayShootEffects()
    {
        shootEffect?.Play();
        var initialPitch = src.pitch;
        src.pitch = src.pitch + Random.Range(-stats.pitchRandomness, stats.pitchRandomness);
        //remove comment after added audioclip
        src.PlayOneShot(stats.shootSFX);
        src.pitch = initialPitch;
    }

    public override void Aim(Vector2 aimPoint)
    {

    }

    public override void WasTossedAway()
    {
        base.WasTossedAway();
        StopAllCoroutines();
    }

    public override void Use()
    {
        Shoot();
    }
}
