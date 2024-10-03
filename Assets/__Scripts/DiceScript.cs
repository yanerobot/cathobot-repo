using UnityEngine;
using UnityEngine.Events;

public class DiceScript : MonoBehaviour
{
    [SerializeField] PowerUp powerUp;
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip debuffClip, buffClip;

    bool enteredHitZone;

    [SerializeField] float initialDiceDelay;
    [SerializeField] float diceCD = 5;
    [SerializeField] float diceStartZone = 1.5f;
    [SerializeField] float diceHitZone = 0.5f;

    float currentCD;

    bool diceStarted;

    SafeZone safeZone;

    public UnityAction OnStartRolling;
    public UnityAction OnHitZoneEnter;
    public UnityAction<BuffInfo> OnFinishRolling;
    public UnityAction OnStopRolling;

    public const string TAG = "RollingDice";

    bool canIgnoreTimescale;

    private void Start()
    {
        currentCD = diceCD;

        SafeZone.OnSafeZoneExit.AddListener(StartRolling);
    }

    void Update()
    {
        if (diceStarted && Input.GetKeyDown(KeyCode.Mouse1) && (Time.timeScale > 0 || canIgnoreTimescale))
        {
            canIgnoreTimescale = false;
            CancelInvoke();
            CheckSuccess();
        }

        if (UIBehaiv.LevelEnded)
            StopRolling();
    }

    public void IgnoreTimescale()
    {
        canIgnoreTimescale = true;
    }

    public void StartRolling()
    {
        diceStarted = true;
        OnStartRolling?.Invoke();
        Invoke(nameof(DiceHitZone), diceStartZone);
    }

    public void DiceHitZone()
    {
        enteredHitZone = true;
        src.Play();
        OnHitZoneEnter?.Invoke();
        Invoke(nameof(DiceEnd), diceHitZone);
    }

    public void DiceEnd()
    {
        enteredHitZone = false;
        CheckSuccess();
    }

    public void CheckSuccess()
    {
        BuffInfo buffInfo;

        if (enteredHitZone)
        {
            src.PlayOneShot(buffClip);
            buffInfo = powerUp.ChoosePowerUp(true);
        }
        else
        {
            src.PlayOneShot(debuffClip);
            buffInfo = powerUp.ChoosePowerUp(false);
        }


        OnFinishRolling?.Invoke(buffInfo);

        currentCD = diceCD  + buffInfo.time;

        diceStartZone = Random.Range(1f, 2f);

        Invoke(nameof(StartRolling), currentCD);

        diceStarted = false;
        enteredHitZone = false;
    }

    public void StopRolling()
    {
        CancelInvoke(); 
        OnStopRolling?.Invoke();
        diceStarted = false;
    }
}
