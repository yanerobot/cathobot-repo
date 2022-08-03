using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Image fillImage;
    [SerializeField] Vector3 offset;

    AIManager aiManager;

    void Start()
    {
        health._OnDamage.AddListener(DisplayCurrentHealth);
        health._OnRestore.AddListener(DisplayCurrentHealth);
        DisplayCurrentHealth(0);
    }

    void LateUpdate()
    {
        transform.eulerAngles *= 0;
        transform.position = health.transform.position + offset;
    }

    void DisplayCurrentHealth(int damage)
    {
        fillImage.fillAmount = (float)health.currentHealth / (float)health.maxHealth;
    }
}
