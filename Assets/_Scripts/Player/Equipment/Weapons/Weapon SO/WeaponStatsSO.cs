using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "SO/Weapon/WeaponStats")]
public class WeaponStatsSO : ScriptableObject
{
    public GameObject bulletPrefab;

    [Header("Stats")]
    public bool isAutomatic;
/*    public bool isShotgun;
    public float shotgunSpreadAngle;
    public int shotgunMaxBulletsPershot;*/
    public float delayBetweenShots;
    public int damage;
    public float bulletSpeed;

    [Header("Audio")]
    public AudioClip shootSFX;
    public float pitchRandomness;
}
