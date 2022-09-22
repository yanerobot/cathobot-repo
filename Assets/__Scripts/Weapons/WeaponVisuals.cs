using UnityEngine;

public class WeaponVisuals : MonoBehaviour
{
    [SerializeField] GameObject equippedGFX, nonEquippedGFX;
    [SerializeField] Animator animator;
    Weapon weapon;

    void Awake()
    {
        weapon = GetComponent<Weapon>();
        if (weapon == null)
            throw new System.Exception("Weapon script should be attached");

        SetGFX(weapon.IsEquipped);

        weapon.OnEquipAction += SetGFX;
        if (animator != null)
            weapon.OnShoot += OnShoot;
    }

    void OnDestroy()
    {
        weapon.OnEquipAction -= SetGFX;
        if (animator != null)
            weapon.OnShoot -= OnShoot;
    }

    void SetGFX(bool isEquipped)
    {
        equippedGFX.SetActive(isEquipped);
        nonEquippedGFX.SetActive(!isEquipped);
    }

    void OnShoot()
    {
        animator.SetTrigger("Shot");
    }

    public Sprite GetNonEquippedSprite()
    {
        return nonEquippedGFX.GetComponent<SpriteRenderer>().sprite;
    }
}
