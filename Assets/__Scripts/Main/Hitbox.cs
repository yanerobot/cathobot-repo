using UnityEngine;

public class Hitbox : MonoBehaviour, IBuffable
{
    [SerializeField] CharacterGFXBehaivior gfx;
    [SerializeField] CapsuleCollider2D mainCollider;
    [SerializeField] Transform GFX;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] AudioClip onDamageClip;
    [SerializeField] AudioSource src;
    [SerializeField] Health health;

    Vector2 ccNormalSize;
    Vector2 gfxNormalSize;

    [HideInInspector]
    public bool isTightPlace;

    void Start()
    {
        ccNormalSize = mainCollider.size;
        gfxNormalSize = GFX.localScale;
    }

    public void Buff(float value, float time)
    {
        mainCollider.size *= value;
        GFX.localScale *= value;

        Invoke(nameof(SetNormalModifier), time);
    }

    public void SetNormalModifier()
    {
        mainCollider.size = ccNormalSize;
        GFX.localScale = gfxNormalSize;

        if (isTightPlace)
            this.Co_DelayedExecute(() => health.Kill(true), 0.1f);
    }

    public void OnDie()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity).transform.localScale = transform.localScale;
        Destroy(gameObject);
    }

    public void OnDamage()
    {
        StopAllCoroutines();
        gfx.Blink();
        src.PlayOneShot(onDamageClip, 1.8f);
    }
}
