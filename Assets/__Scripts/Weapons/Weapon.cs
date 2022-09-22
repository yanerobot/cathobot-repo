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
    int currentMagBullets;
    int currentBullets;

    public UnityAction<(int, int)> OnChangeBullets;
    public UnityAction OnReload;
    public UnityAction OnShoot;

    protected override void Awake()
    {
        base.Awake();
        GFX = GetComponentInChildren<SpriteRenderer>();
        src = GetComponent<AudioSource>();
        currentMagBullets = -1;
        if (IsReloadable())
        {
            currentBullets = stats.maxBullets;
            currentMagBullets = stats.magBullets;
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
        if (reloading || !IsReloadable() || IsEmpty())
            return;

        reloading = true;
        this.Co_DelayedExecute(FinishReload, stats.reloadTime);
        character?.OnReloadStart?.Invoke(stats.reloadTime);
    }

    void FinishReload()
    {
        reloading = false;
        currentMagBullets = stats.magBullets;
        OnChangeBullets?.Invoke(GetBullets());
    }

    protected bool HasBullets()
    {
        if (!IsReloadable())
            return true;

        if (reloading)
            return false;

        if (IsEmpty())
            return false;

        return currentMagBullets > 0;
    }

    protected void EnableShooting()
    {
        canShoot = true;
    }

    protected virtual void PlayShootEffects()
    {
        //shootEffect?.Play();
        var initialPitch = src.pitch;
        src.pitch = src.pitch + Random.Range(-stats.pitchRandomness, stats.pitchRandomness);
        src.PlayOneShot(stats.shootSFX);
        src.pitch = initialPitch;
    }

    public (int, int) GetBullets()
    {
        if (HasBulletCap())
            return (currentMagBullets, currentBullets);
        return (currentMagBullets, stats.magBullets);
    }

    protected void SpendBullets(int amount)
    {
        if (IsEmpty())
            return;

        currentMagBullets -= amount;
        currentBullets -= amount;
        OnShoot?.Invoke();

        if (!IsReloadable())
            return;

        OnChangeBullets?.Invoke(GetBullets());

        if (currentMagBullets <= 0)
        {
            Reload();
        }
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
        if (reloading)
            FinishReload();
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

    bool IsEmpty()
    {
        return HasBulletCap() && currentBullets <= 0;
    }

    bool HasBulletCap() => stats.maxBullets > 0;
}
