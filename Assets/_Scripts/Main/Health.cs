using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    [SerializeField] float InvulnerabilityAfterHit;
    public int maxHealth;
    public int initialHealth;
    public bool isStatic;
    [SerializeField] GameObject invBubble;
    public int currentHealth { get; private set; }

    public bool isDead { get; private set; }

    [Tooltip("Called even if target is dead")]
    public UnityEvent _OnRegisterHit;
    public UnityEvent<int> _OnDamage, _OnRestore;
    public UnityEvent _OnDie;

    bool invulnerable;
    float timeAfterFirstHit;
    bool startCountingTime;
    float damageModifier = 1;

    Dictionary<string, Coroutine> tickingSourcesCoroutines;

    void OnEnable()
    {
        Init();
    }

    void Update()
    {
        if (startCountingTime)
        {
            if (timeAfterFirstHit >= InvulnerabilityAfterHit)
            {
                startCountingTime = false;
                invulnerable = false;
            }
            else
            {
                timeAfterFirstHit += Time.deltaTime;
            }
        }
    }

    public virtual void Init()
    {
        if (initialHealth > 0)
            currentHealth = initialHealth;
        else
            currentHealth = maxHealth;

        isDead = false;
    }
#if UNITY_EDITOR
    [ContextMenu("Take Damage")]
    public void TakeDamageTest()
    {
        TakeDamage(10);
    }

#endif
    public void Kill()
    {
        TakeDamage(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        _OnRegisterHit?.Invoke();

        if (isDead) 
            return;

        if (invulnerable)
            return;

        timeAfterFirstHit = 0;
        startCountingTime = true;
        if (InvulnerabilityAfterHit > 0)
            invulnerable = true;

        currentHealth -= (int)(amount * damageModifier);

        if (currentHealth < 0) 
            currentHealth = 0;


        if (currentHealth == 0)
        {
            isDead = true;
            _OnDie?.Invoke();
            return;
            
        }

        _OnDamage?.Invoke(currentHealth);
    }

    public void Restore(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        _OnRestore?.Invoke(currentHealth);
    }

    public void TickDamage(float tickRate, int damage, Func<bool> predicate, int id)
    {
        if (tickingSourcesCoroutines == null)
            tickingSourcesCoroutines = new Dictionary<string, Coroutine>();

        var coroutine = StartCoroutine(Co_TickDamage(tickRate, damage, predicate));

        string idStr = id + "" + transform.GetInstanceID();

        if (tickingSourcesCoroutines.ContainsKey(idStr))
        {
            tickingSourcesCoroutines[idStr] = coroutine;
        }
        else
        {
            tickingSourcesCoroutines.Add(idStr, coroutine);
        }
    }

    public void StopTickDamage(int id)
    {
        string idStr = id + "" + transform.GetInstanceID();

        tickingSourcesCoroutines.TryGetValue(idStr, out var coroutine);

        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    IEnumerator Co_TickDamage(float tickRate, int damage, Func<bool> predicate)
    {
        while (predicate() == false)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(tickRate);
        }
    }

    public void BuffHealth(float modifier, float time)
    {
        damageModifier = modifier;
        Invoke(nameof(SetNormalHealth), time);
    }

    void SetNormalHealth()
    {
        damageModifier = 1;
    }

    public void MakeInvulnirable(float time)
    {
        invBubble.gameObject.SetActive(true);
        invulnerable = true;
        Invoke(nameof(ResetInvul), time);
    }

    void ResetInvul()
    {
        invulnerable = false;
        invBubble.gameObject.SetActive(false);
    }
}
