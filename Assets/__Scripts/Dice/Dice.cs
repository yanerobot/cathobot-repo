using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] int totalBuffCount;
    [SerializeField] float cooldown;
    [SerializeField] float initialRollRate, rollRateModifierPerTick;
    [SerializeField] PowerUp powerUp;

    int currentBuff;

    bool isRolling;
    RollingDiceUI diceUI;

    public bool canRoll = true;

    void Start()
    {
        /*var diceUiGo = GameObject.FindWithTag(RollingDiceUI.TAG);
        if (diceUiGo == null)
            return;
*//*
        diceUI = diceUiGo.GetComponent<RollingDiceUI>();
*/
        LevelStartCountDown.OnCountDownEnd.AddListener(StartRolling);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isRolling && canRoll)
        {
            StopAllCoroutines();
            FinishRolling();
        }
    }

    void StartRolling()
    {
        Roll();
    }

    void Roll() => StartCoroutine(Co_StartRolling());

    void FinishRolling()
    {
/*        isRolling = false;

        //var bufftime = powerUp.ChoosePowerUp(currentBuff);

        Invoke(nameof(Roll), cooldown + bufftime);

        diceUI.OnFinishRolling(bufftime, currentBuff);*/
    }

    IEnumerator Co_StartRolling()
    {
        diceUI.OnStartRolling();
        isRolling = true;
        int startBuff = Random.Range(0, totalBuffCount);
        int index = 0;
        int totalSecondsPassed = 0;
        float currentRollRate = initialRollRate;

        while (index < totalBuffCount * 2)
        {
            currentBuff = startBuff;
            diceUI.OnDiceSwitch(currentBuff);
            yield return new WaitForSeconds(currentRollRate);
            currentRollRate *= rollRateModifierPerTick;
            startBuff++;
            index++;
            startBuff %= totalBuffCount;
            totalSecondsPassed += 1;
        }

        FinishRolling();
    }

    public void StopDice()
    {
        canRoll = false;
        StopAllCoroutines();
    }
}
