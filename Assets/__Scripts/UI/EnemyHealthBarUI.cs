using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Image fillImage;

    AIManager aiManager;

    void Start()
    {
        health._OnDamage.AddListener(DisplayCurrentHealth);
        health._OnRestore.AddListener(DisplayCurrentHealth);
        DisplayCurrentHealth(0);
    }

    void DisplayCurrentHealth(int damage)
    {
        fillImage.fillAmount = (float)health.currentHealth / (float)health.maxHealth;
    }
}
