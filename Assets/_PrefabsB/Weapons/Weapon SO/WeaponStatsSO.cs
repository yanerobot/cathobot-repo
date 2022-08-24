using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "SO/Weapon/WeaponStats")]
public class WeaponStatsSO : ScriptableObject
{
    public Bullet bulletPref;

    [Header("Stats")]
    public bool isAutomatic;
    public float delayBetweenShots;
    public int damage;
    public float bulletSpeed;
    public float randomBulletSpread;

    [Header("Reload")]
    public int magBullets;
    public int maxBullets;
    public float reloadTime;

    [Header("Audio")]
    public AudioClip shootSFX;
    public float pitchRandomness;
}
