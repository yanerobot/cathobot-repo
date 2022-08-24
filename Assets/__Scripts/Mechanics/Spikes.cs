using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] int damage;
    [SerializeField] float tickRate;

    [SerializeField] AudioSource src;
    [SerializeField] AudioClip onStepSound, activationSound;
    [SerializeField] Sprite openSpikesSprite;

    Sprite initialSprite;
    int currentNum;

    const string spikesTickDamageKey = "spikes";

    List<Health> takingDamage;

    void Start()
    {
        initialSprite = sr.sprite;

        takingDamage = new List<Health>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            health.TickDamage(spikesTickDamageKey, tickRate, damage);
            takingDamage.Add(health);
            src.Play();
            if (currentNum == 0)
            {
                src.PlayOneShot(activationSound);
                src.PlayOneShot(onStepSound);
                sr.sprite = openSpikesSprite;
            }
            currentNum++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            if (takingDamage.Contains(health))
            {
                takingDamage.Remove(health);
                health?.StopTickDamage(spikesTickDamageKey);
            }

            currentNum--;

            if (currentNum == 0)
            {
                sr.sprite = initialSprite;
                src.PlayOneShot(activationSound);
            }
        }
    }
}
