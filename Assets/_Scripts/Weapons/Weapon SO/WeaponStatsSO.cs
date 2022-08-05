using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "SO/Weapon/WeaponStats")]
public class WeaponStatsSO : ScriptableObject
{
    public GameObject bulletPrefab;

    [Header("Stats")]
    public bool isAutomatic;
    public float delayBetweenShots;
    public int damage;
    public float bulletSpeed;

    [Header("Reload")]
    public int maxBullets;
    public float reloadTime;

    [Header("Audio")]
    public AudioClip shootSFX;
    public float pitchRandomness;
}
