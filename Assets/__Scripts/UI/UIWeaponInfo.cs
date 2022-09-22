using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWeaponInfo : MonoBehaviour
{
    [SerializeField] GameObject bulletCountGO;
    [SerializeField] GameObject infniteAmmoGO;
    [SerializeField] TextMeshProUGUI bulletTextObj;
    [SerializeField] Image weaponImage;
    [SerializeField] UIBehaiv ui;
    EquipmentSystem es;

    Weapon currentWeapon;

    void Start()
    {
        var player = ui.GetPlayerHealth();

        es = player.GetComponent<EquipmentSystem>();
        es.OnEquip += EnableUI;
        es.OnToss += DisableUI;
    }

    void EnableUI(Item item)
    {
        if (!(item is Weapon weapon))
            return;

        currentWeapon = weapon;

        var weaponVisuals = currentWeapon.GetComponent<WeaponVisuals>();

        weaponImage.sprite = currentWeapon.GetComponent<WeaponVisuals>().GetNonEquippedSprite();
        weaponImage.gameObject.SetActive(true);

        if (!currentWeapon.IsReloadable())
        {
            infniteAmmoGO.SetActive(true);
            return;
        }

        var bullets = currentWeapon.GetBullets();
        currentWeapon.OnChangeBullets += DisplayBullets;

        bulletCountGO.SetActive(true);
        DisplayBullets(bullets);
    }

    void DisableUI(Item item)
    {
        bulletCountGO.SetActive(false);
        infniteAmmoGO.SetActive(false);
        weaponImage.gameObject.SetActive(false);

        if (currentWeapon != null)
            currentWeapon.OnChangeBullets -= DisplayBullets;

        currentWeapon = null;
    }

    void DisplayBullets((int, int) bulletData)
    {
        bulletTextObj.text = bulletData.Item1.ToString() + "/" + bulletData.Item2.ToString();
    }

    void OnDestroy()
    {
        if (es != null)
        {

            es.OnEquip -= EnableUI;
            es.OnToss -= DisableUI;
        }

        if (currentWeapon != null)
        {
            currentWeapon.OnChangeBullets -= DisplayBullets;
        }
    }
}
