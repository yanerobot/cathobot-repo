using UnityEngine;

public class TopDownMovement : MonoBehaviour, IStunnable
{
    public const string PLAYERTAG = "Player";

    [SerializeField] Animator animator;
    [SerializeField] float movementSpeed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] AudioSource src;

    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    [Header("Speed Buff After Timed Dash")]
    [SerializeField] float superDashTiming;
    [SerializeField] float superDashDuration;
    [SerializeField] float superDashMultiplier;
    [SerializeField] AudioClip superDashSound;
    
    bool canDash;
    bool isDashing;
    bool superDash;

    float Modifier = 1;
    float superDashModifier = 1;

    Vector2 movementInput;

    bool stunned;

    void Start()
    {
        SafeZone.OnSafeZoneOut.AddListener(()=> canDash = true);
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (UIBehaiv.LevelEnded)
        {
            movementInput *= 0;
            src.Stop();
            animator.SetBool("IsMoving", false);
            return;
        }

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput = movementInput.normalized;

        animator.SetBool("IsMoving", movementInput.magnitude != 0);

        var mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = (Vector2)mouseDir - (Vector2)transform.position;
        transform.up = dir;

        if (canDash && Input.GetKeyDown(KeyCode.Space))
            Dash();
    }
    public void Buff(float newModifier, float time)
    {
        StopDash();
        CancelInvoke();
        StopAllCoroutines();
        src.Stop();
        Modifier = newModifier;
        superDash = false;
        canDash = false;
        Invoke(nameof(SetNormalModifier), time);
    }

    public void SetNormalModifier()
    {
        canDash = true;
        Modifier = 1;
    }

    void FixedUpdate()
    {
        if (isDashing)
            return;

        if (stunned)
        {
            if (rb.velocity.magnitude >= movementSpeed * Modifier)
                return;

            rb.AddForce(movementInput * movementSpeed * Modifier * 0.5f, ForceMode2D.Impulse);
            return;
        }

        rb.velocity = movementInput * movementSpeed * Modifier * superDashModifier;
    }

    void Dash()
    {
        if (superDash)
        {
            superDashModifier = superDashMultiplier;
            this.Co_DelayedExecute(() => superDashModifier = 1, superDashDuration);
            src.PlayOneShot(superDashSound);
        }
        canDash = false;
        isDashing = true;
        
        var dashDir = movementInput;
        
        if (movementInput.magnitude < 0.001f)
            dashDir = transform.up;
        
        rb.velocity = dashDir * dashSpeed * superDashModifier;

        Invoke(nameof(StopDash), dashTime);
        Invoke(nameof(EnableDash), dashTime + dashCooldown);
    }

    void StopDash()
    {
        rb.velocity *= 0;
        isDashing = false;
        src.Play();
    }

    void EnableDash()
    {
        if (UIBehaiv.LevelEnded)
            return;

        src.Stop();
        superDash = true;
        canDash = true;

        this.Co_DelayedExecute(() => superDash = false, superDashTiming);
    }

    Coroutine stunRoutine;
    public void Stun(float time)
    {
        if (stunRoutine != null)
            StopCoroutine(stunRoutine);

        stunned = true;
        stunRoutine = this.Co_DelayedExecute(() => stunned = false, time);
    }
}
