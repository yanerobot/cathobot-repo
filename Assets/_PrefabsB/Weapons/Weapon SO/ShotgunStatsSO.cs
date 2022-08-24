using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "SO/Weapon/ShotgunStats")]
public class ShotgunStatsSO : WeaponStatsSO
{
    [Header("Shotgun")]
    public float shotgunSpreadAngle;
    public int shotgunMaxBulletsPershot;
}
