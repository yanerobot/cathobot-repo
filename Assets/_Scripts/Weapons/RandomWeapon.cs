using UnityEngine;
using System.Collections.Generic;

public class RandomWeapon : Weapon
{
    public List<WeaponStatsSO> gunTypes;
    [SerializeField] float delay;

    public override void WasEquippedBy(EquipmentSystem character)
    {
        base.WasEquippedBy(character);

        InvokeRepeating(nameof(Randomize), 0, delay);
    }

    public override void WasTossedAway()
    {
        base.WasTossedAway();

        CancelInvoke(nameof(Randomize));
    }

    void Randomize()
    {
        var randomStat = gunTypes[Random.Range(0, gunTypes.Count)];

        ChangeStats(randomStat);
    }

    void ChangeStats(WeaponStatsSO newStats)
    {
        stats = newStats;

        StopAllCoroutines();
    }
}
