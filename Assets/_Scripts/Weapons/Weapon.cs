using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Weapon : Item
{
    [Header("Weapon")]
    [SerializeField] protected WeaponStatsSO stats;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] ParticleSystem shootEffect;
    [SerializeField] protected float bulletSpread;

    protected AudioSource src;
    [HideInInspector]
    public SpriteRenderer GFX;

    bool reloading;
    protected bool canShoot = true;
    int currentBullets;
    protected int CurrentBullets
    {
        get
        {
            return currentBullets;
        }
        set
        {
            currentBullets = value;

            if (stats.reloadTime <= 0)
                return;

            OnChangeBullets?.Invoke(GetBullets());

            if (currentBullets <= 0)
            {
                Reload();
            }
        }
    }

    public UnityAction<(int, int)> OnChangeBullets;
    public UnityAction OnReload;

    protected override void Awake()
    {
        base.Awake();
        GFX = GetComponentInChildren<SpriteRenderer>();
        src = GetComponent<AudioSource>();
        CurrentBullets = -1;
        if (stats.reloadTime > 0)
        {
            CurrentBullets = stats.maxBullets;
        }
    }

    protected virtual void Shoot() { }

    protected Bullet SingleShot()
    {
        if ((canShoot == false) && !(this is Automatic))
            return null;

        canShoot = false;

        Invoke(nameof(EnableShooting), stats.delayBetweenShots);

        PlayShootEffects();

        return CreateBullet();
    }

    protected Bullet CreateBullet(float spreadAngle = 0)
    {
        var bullet = Instantiate(stats.bulletPref, shootingPoint.position, shootingPoint.rotation, null);
        bullet.transform.Rotate(Vector3.forward, spreadAngle + Random.Range(-stats.randomBulletSpread, stats.randomBulletSpread));

        bullet.Init(character.gameObject, stats.damage, stats.bulletSpeed, character.Modifier);

        return bullet;
    }

    public void Reload()
    {
        if (reloading || !IsReloadable())
            return;

        reloading = true;
        this.Co_DelayedExecute(FinishReload, stats.reloadTime);
        character?.OnReloadStart?.Invoke(stats.reloadTime);
    }

    void FinishReload()
    {
        reloading = false;
        CurrentBullets = stats.maxBullets;
    }

    protected bool HasBullets()
    {
        if (stats.reloadTime <= 0)
            return true;

        return CurrentBullets > 0;
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

    public (int, int) GetBullets()
    {
        return (CurrentBullets, stats.maxBullets);
    }

    public override void WasEquippedBy(EquipmentSystem character)
    {
        base.WasEquippedBy(character);

        if (!HasBullets())
            this.Co_DelayedExecute(Reload, stats.reloadTime);
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

    public bool IsReloadable()
    {
        return stats.reloadTime > 0;
    }
}
