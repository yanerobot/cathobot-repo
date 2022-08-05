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

    int currentBullets = -1;

    protected override void Awake()
    {
        base.Awake();
        GFX = GetComponentInChildren<SpriteRenderer>();
        src = GetComponent<AudioSource>();
        if (stats.reloadTime > 0)
        {
            currentBullets = stats.maxBullets;
        }
    }

    protected virtual void Shoot()
    {
        SingleShot();
    }

    protected void SingleShot()
    {
        if ((canShoot == false) && !(this is Automatic))
            return;


        canShoot = false;
        

        Invoke(nameof(EnableShooting), stats.delayBetweenShots);

        var bulletGO = Instantiate(stats.bulletPrefab, shootingPoint.position, shootingPoint.rotation, null);
        var bullet = bulletGO.GetComponent<Bullet>();

        bullet.Init(character.gameObject, stats.damage, stats.bulletSpeed, character.Modifier);

        PlayShootEffects();
    }

    void Reload()
    {
        currentBullets = stats.maxBullets;
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
        src.PlayOneShot(stats.shootSFX);
        src.pitch = initialPitch;
    }

    public override void WasEquippedBy(EquipmentSystem character)
    {
        base.WasEquippedBy(character);

        if (currentBullets == 0)
            this.Co_DelayedExecute(Reload, stats.reloadTime);
    }

    public override void WasTossedAway()
    {
        base.WasTossedAway();
        StopAllCoroutines();
    }

    public override void Use()
    {
        if (stats.reloadTime > 0)
        {
            if (currentBullets <= 0)
                return;

            currentBullets--;

            if (currentBullets == 0)
                this.Co_DelayedExecute(Reload, stats.reloadTime);
        }

        Shoot();
    }
}
