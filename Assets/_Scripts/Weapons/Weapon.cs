using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Weapon : Item
{
    [Header("Weapon")]
    [SerializeField] protected WeaponStatsSO stats;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] ParticleSystem shootEffect;

    protected AudioSource src;
    [HideInInspector]
    public SpriteRenderer GFX;

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
            if (currentBullets <= 0)
                Reload();
            OnChangeBullets?.Invoke(GetBullets());
        }
    }

    public UnityAction<(int, int)> OnChangeBullets;

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

        var bulletGO = Instantiate(stats.bulletPrefab, shootingPoint.position, shootingPoint.rotation, null);
        var bullet = bulletGO.GetComponent<Bullet>();

        bullet.Init(character.gameObject, stats.damage, stats.bulletSpeed, character.Modifier);

        PlayShootEffects();

        return bullet;
    }

    protected void Reload()
    {
        this.Co_DelayedExecute(() => CurrentBullets = stats.maxBullets, stats.reloadTime);
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

        if (CurrentBullets == 0)
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
