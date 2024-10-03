using UnityEngine;
using System.Collections.Generic;
public class TopDownMovement : MonoBehaviour, IStunnable, IIceBehaivior
{
    public const string PLAYERTAG = "Player";

    [SerializeField] Animator animator;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxSpeed;
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
    [SerializeField] LayerMask bounceLayers;
    [SerializeField] float slowDownRate;
    
    bool canDash;
    bool gameStarted;
    bool isDashing;
    bool superDash;

    float Modifier = 1;
    float superDashModifier = 1;

    Vector2 movementInput;

    bool stunned;
    bool onIce;
    bool speedBoosted;

    List<float> allBuffs;
    int currentBuffIndex;

    float currentSpeed;
    bool slowDown;

    public Vector2 MovementVector => rb.velocity;


    void Start()
    {
        SafeZone.OnSafeZoneExit.AddListener(()=> {
            gameStarted = true;
            canDash = true;
        });
        allBuffs = new List<float>();
        allBuffs.Add(1);

        currentSpeed = movementSpeed;
    }

    void Update()
    {
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

    void LateUpdate()
    {
        if (isDashing)
            return;

        if (stunned || onIce)
        {
            if (rb.velocity.magnitude >= movementSpeed * Modifier)
                return;

            //rb.AddForce(movementInput * movementSpeed * Modifier * 0.5f, ForceMode2D.Impulse);
            rb.AddForce(movementInput * Modifier, ForceMode2D.Impulse);
            return;
        }
        rb.velocity = movementInput * currentSpeed * Modifier * superDashModifier;

        if (currentSpeed > movementSpeed) 
        {
            currentSpeed = Mathf.Clamp(rb.velocity.magnitude, movementSpeed, currentSpeed);
        }

        if (slowDown)
        {
            currentSpeed -= slowDownRate;
            currentSpeed = Mathf.Clamp(currentSpeed, movementSpeed, maxSpeed);
            if (currentSpeed == movementSpeed)
                slowDown = false;
        }
    }
    public void Buff(float newModifier, float time)
    {
        ResetMovementModifiers();
        Modifier = newModifier;
        canDash = false;
        Invoke(nameof(EnableDash), time);
        Invoke(nameof(SetNormalModifier), time);
    }

    public void SpeedBoost(float additionalSpeed, float time)
    {
        ResetMovementModifiers();
        canDash = false;
        slowDown = false;
        currentSpeed += additionalSpeed;
        if (currentSpeed > maxSpeed)
            currentSpeed = maxSpeed;

        Invoke(nameof(EnableDash), dashCooldown);
        Invoke(nameof(SlowDown), 1f);
    }

    void SlowDown() => slowDown = true;

    public void SetNormalModifier()
    {
        Modifier = 1;
    }

    void ResetMovementModifiers()
    {
        CancelInvoke();
        src.Stop();
        isDashing = false;
        superDash = false;
        stunned = false;
        superDashModifier = 1;
    }

    #region Dash

    void Dash()
    {
        if (superDash)
        {
            superDashModifier = superDashMultiplier;
            Invoke(nameof(ResetSuperDash), superDashDuration);
            src.PlayOneShot(superDashSound);
        }
        canDash = false;
        isDashing = true;

        Physics2D.IgnoreLayerCollision(gameObject.layer, 6, true);
        
        var dashDir = movementInput;
        
        if (movementInput.magnitude < 0.001f)
            dashDir = transform.up;
        
        rb.velocity = dashDir * dashSpeed * superDashModifier;

        Invoke(nameof(StopDash), dashTime);
    }

    void StopDash()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 6, false);
        isDashing = false;
        src.Play();
        Invoke(nameof(EnableDash), dashCooldown);
        Invoke(nameof(EnableSuperDash), dashCooldown);
    }

    void EnableDash()
    {
        if (UIBehaiv.LevelEnded)
            return;

        src.Stop();
        canDash = true;
    }

    void EnableSuperDash()
    {
        superDash = true;
        Invoke(nameof(DisableSuperDash), superDashTiming);
    }

    void DisableSuperDash()
    {
        superDash = false;
    }

    void ResetSuperDash()
    {
        superDashModifier = 1;
    }
    #endregion

    #region Stun
    public void DisableMovement(float time)
    {
        CancelInvoke(nameof(EnableMovement));

        stunned = true;
        Invoke(nameof(EnableMovement), time);
    }

    void EnableMovement()
    {
        stunned = false;
    }
    #endregion

    #region Ice
    float initialDrag;
    public void OnIceEnter()
    {
        onIce = true;
        initialDrag = rb.drag;
        rb.velocity *= 0.5f;
        rb.drag *= 0.2f;
    }

    public void OnIceExit()
    {
        onIce = false;
        rb.drag = initialDrag;
    }
    #endregion
}
