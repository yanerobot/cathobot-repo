using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
    [SerializeField] PowerUp powerUp;
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip debuffClip, buffClip;

    bool enteredHitZone;

    public float diceCD = 5;
    public float diceStartZone = 1.5f;
    public float diceHitZone = 0.5f;

    RollingDiceUI diceUI;

    float currentCD;

    bool diceStarted;

    LevelStartCountDown safeZone;


    private void Start()
    {
        currentCD = diceCD;
        var obj = GameObject.FindWithTag(RollingDiceUI.TAG);
        if (obj == null)
        {
            gameObject.SetActive(false);
            return;
        }
        diceUI = obj.GetComponent<RollingDiceUI>();

        LevelStartCountDown.OnCountDownEnd.AddListener(DiceStart);
    }

    void Update()
    {
        if (diceStarted && Input.GetKeyDown(KeyCode.Mouse1))
        {
            CancelInvoke();
            CheckSuccess();
        }

        if (UIBehaiv.LevelEnded)
            StopRolling();
    }


    public void DiceStart()
    {
        diceStarted = true;
        diceUI.OnStartRolling();
        Invoke(nameof(DiceHitZone), diceStartZone);
    }

    public void DiceHitZone()
    {
        enteredHitZone = true;
        src.Play();
        diceUI.OnHitZoneEnter();
        Invoke(nameof(DiceEnd), diceHitZone);
    }

    public void DiceEnd()
    {
        enteredHitZone = false;
        CheckSuccess();
    }

    public void CheckSuccess()
    {
        (float, int) buffInfo = (0, 0);

        if (enteredHitZone)
        {
            print("buff");
            src.PlayOneShot(buffClip);
            buffInfo = powerUp.ChoosePowerUp(true);
        }
        else
        {
            print("debuff");
            src.PlayOneShot(debuffClip);
            buffInfo = powerUp.ChoosePowerUp(false);
        }


        diceUI.OnFinishRolling(buffInfo.Item1, buffInfo.Item2, enteredHitZone);

        currentCD = diceCD  + buffInfo.Item1;

        diceStartZone = Random.Range(1f, 3f);

        Invoke(nameof(DiceStart), currentCD);

        diceStarted = false;
        enteredHitZone = false;
    }

    public void StopRolling()
    {
        CancelInvoke();
    }
}