using UnityEngine;

public class AI_DashAbilitySpecial : AI_SpecialAbility
{
    [SerializeField] float dashCooldownMin;
    [SerializeField] float dashCooldownMax;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashPreparation;
    [SerializeField] float pauseAfterPrep;
    [SerializeField] float dashTime;
    [SerializeField] int damageOnHit;
    [SerializeField] LineRenderer lineRendererPrefab;

    bool isDashing;
    bool isPreparing;
    bool hasDealtDamage;
    bool hasDashed;
    float initialDrag;

    TimeCondition timer;
    LineRenderer lineRend;

    Vector2 targetLastPos;

    public override void OnSpecialEnter()
    {
        StartDash();
        isDashing = true;
        if (AI.IsPlayerAvailable())
            targetLastPos = AI.target.position;
    }

    public override void OnSpecialUpdate()
    {
        if (isPreparing)
        {
            transform.up = GetLastTargetPosition(AI.target) - (Vector2)transform.position;
            var hit = Physics2D.Raycast(transform.position, transform.up, 30, AI.visibleLayers);
            if (hit.collider != null)
            {
                lineRend.SetPosition(1, Vector3.zero.WhereY(hit.distance));
            }
        }
    }

    public override void OnSpecialExit()
    {
        hasDashed = false;
    }
    void StartDash()
    {
        hasDealtDamage = false;
        lineRend = Instantiate(lineRendererPrefab, transform.position, transform.rotation, transform);
        isPreparing = true;
        AI.DisableMovement();
        this.Co_DelayedExecute(StopPreparation, dashPreparation);
    }
    void StopPreparation()
    {
        isPreparing = false;
        this.Co_DelayedExecute(Dash, pauseAfterPrep);
    }

    void Dash()
    {
        Destroy(lineRend.gameObject);
        initialDrag = AI.rb.drag;
        AI.rb.drag = 0;
        AI.rb.velocity = transform.up * dashSpeed;
        this.Co_DelayedExecute(StopDash, dashTime);
    }

    void StopDash()
    {
        AI.EnableMovement();
        AI.rb.velocity *= 0;
        AI.rb.drag = initialDrag;
        timer.ResetTimer();
        isDashing = false;
        hasDashed = true;
    }

    Vector2 GetLastTargetPosition(Transform target)
    {
        var hit = Physics2D.Raycast(transform.position, transform.position.Direction(target.position), 50, AI.visibleLayers);
        
        if (hit.collider.transform == target.transform)
        {
            targetLastPos = AI.target.position;
        }

        return targetLastPos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDashing || isPreparing || hasDealtDamage)
            return;

        if (collision.transform.TryGetComponent(out Health health))
        {
            hasDealtDamage = true;
            health.TakeDamage(damageOnHit);
        }
    }
    public override bool EnterCondition()
    {
        return  AI.IsPlayerVisible() && timer.HasTimePassed() && AI.IsPlayerWithinRunningRange();
    }

    public override bool ExitCondition()
    {
        return hasDashed;
    }

    public override void ResetConditions()
    {
        timer.ResetTimer();
        timer.ReduceTime(Random.Range(0, dashCooldownMax - dashCooldownMin));
        hasDashed = false;
    }

    public override void InitiateConditions()
    {
        timer = new TimeCondition(dashCooldownMax);
        timer.ReduceTime(Random.Range(0, dashCooldownMax));
    }
}
