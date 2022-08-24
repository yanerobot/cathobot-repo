using UnityEngine;

public class AI_DashAbilitySpecial : MonoBehaviour
{
    [SerializeField] EnemyAI AI;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashPreparation;
    [SerializeField] float pauseAfterPrep;
    [SerializeField] float dashTime;
    [SerializeField] int damageOnHit;
    [SerializeField] GameObject lineRendererPrefab;

    bool isDashing;
    bool isPreparing;
    bool hasDealtDamage;
    float initialDrag;

    TimeCondition timer;
    GameObject lineRend;

    Vector2 targetLastPos;

    void Start()
    {
        AI.OnRunningEnter += OnRunningEnter;
        AI.OnRunningUpdate += OnRunningUpdate;
    }

    void OnRunningEnter()
    {
        timer = new TimeCondition(dashCooldown);
        timer.SetInitialDelay(Random.Range(1, dashCooldown));
        if (AI.IsPlayerAvailable())
            targetLastPos = AI.target.position;
    }

    void OnRunningUpdate()
    {
        if (timer.HasTimePassed() && !isDashing)
        {
            if (AI.IsPlayerVisible())
            {
                StartDash();
                isDashing = true;
            }
        }
        if (isPreparing)
        {
            transform.up = GetLastTargetPosition(AI.target) - (Vector2)transform.position;
        }
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
        Destroy(lineRend);
        initialDrag = AI.rb.drag;
        AI.rb.drag = 0;
        AI.rb.velocity = transform.up * dashSpeed;
        this.Co_DelayedExecute(StopDash, dashTime);
    }

    void StopDash()
    {
        AI.EnableMovement();
        AI.rb.drag = initialDrag;
        timer.ResetTimer();
        isDashing = false;
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

        hasDealtDamage = true;

        if (collision.transform.TryGetComponent(out Health health))
        {
            health.TakeDamage(damageOnHit);
        }
    }

    void OnDestroy()
    {
        AI.OnRunningEnter -= OnRunningEnter;
        AI.OnRunningUpdate -= OnRunningUpdate;
    }
}
