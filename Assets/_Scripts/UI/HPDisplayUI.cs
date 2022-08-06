using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HPDisplayUI : MonoBehaviour
{
    [SerializeField] Image hpFill;
    [SerializeField] TextMeshProUGUI textObj;
    [SerializeField] UIBehaiv ui;
    Health health;

    void Start()
    {
        health = ui.GetPlayerHealth();
        Init();
    }

    void OnDestroy()
    {
        health?._OnDamage.RemoveListener(DisplayHealth);
        health?._OnRestore.RemoveListener(DisplayHealth);
        health?._OnDie.RemoveListener(DisplayZeroHealth);
    }

    void Init()
    {
        hpFill.fillAmount = health.currentHealth / health.maxHealth;

        health._OnDamage.AddListener(DisplayHealth);
        health._OnRestore.AddListener(DisplayHealth);
        health._OnDie.AddListener(DisplayZeroHealth);
    }

    void DisplayZeroHealth()
    {
        hpFill.fillAmount = 0;
    }

    void DisplayHealth(int newHealth)
    {
        var newFillValue = (float)newHealth / health.maxHealth;
        textObj.text = newHealth.ToString();
        hpFill.fillAmount = newFillValue;
    }
}
