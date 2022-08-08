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

    Dictionary<string, TickDamageSource> tickingSources;

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
        tickingSources = new Dictionary<string, TickDamageSource>();

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

    public void TickDamage(string key, float tickRate, int damage)
    {
        print("Start tick damage");
        if (tickingSources.ContainsKey(key))
        {
            if (tickingSources[key].toStop != null)
                StopCoroutine(tickingSources[key].toStop);
        }
        else
        {
            var coroutine = StartCoroutine(Co_TickDamage(tickRate, damage));
            tickingSources.Add(key, new TickDamageSource(coroutine));
        }
    }

    public void StopTickDamage(string key)
    {
        tickingSources.TryGetValue(key, out var src);

        if (src == null)
        {
            tickingSources.Remove(key);
            return;
        }

        if (src.coroutine != null)
        {
            StopCoroutine(src.coroutine);
            tickingSources.Remove(key);
        }

    }
    public void StopTickDamage(string key, float time, UnityAction callback = null)
    {
        tickingSources.TryGetValue(key, out var src);

        if (src == null)
        {
            tickingSources.Remove(key);
            return;
        }

        if (src.toStop != null)
            StopCoroutine(src.toStop);

        src.toStop = this.Co_DelayedExecute(() => 
        {
            if (src == null)
                return;

            if (src.coroutine != null)
            {
                StopCoroutine(src.coroutine);
                tickingSources.Remove(key);
            }
            callback?.Invoke();
        }, time);
    }

    IEnumerator Co_TickDamage(float tickRate, int damage)
    {
        while (true)
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

public class TickDamageSource
{
    public Coroutine coroutine;
    public Coroutine toStop;

    public TickDamageSource (Coroutine routine)
    {
        coroutine = routine;
    }
}
