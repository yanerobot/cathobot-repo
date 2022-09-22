using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class RollingDiceUI : MonoBehaviour
{
    [SerializeField] Color untimedColorMod;
    [SerializeField] Color timedColorMod;
    [SerializeField] Color timeHitColor;
    [SerializeField] Color originalDiceColor;
    [SerializeField] Image buffImg;
    [SerializeField] Image diceImg;
    [SerializeField] Image mouseHint;
    //[SerializeField] Color hitZoneColor;
    [SerializeField, Tooltip("Index should match with Buff Type")] List<Buff> buffSprites;
    [SerializeField] AudioSource src;
    [SerializeField] Color disabledColor;
    [SerializeField] Animator rollingDiceAnimator;

    Buff currentBuff;
    DiceScript diceScript;
    public static bool IsRolling;

    void Start()
    {
        IsRolling = false;
        var go = GameObject.FindWithTag(DiceScript.TAG);
        if (go == null)
        {
            gameObject.SetActive(false);
            return;
        }
        IsRolling = true;

        diceScript = go.GetComponent<DiceScript>();

        diceScript.OnStartRolling += OnStartRolling;
        diceScript.OnHitZoneEnter += OnHitZoneEnter;
        diceScript.OnFinishRolling += OnFinishRolling;
        diceScript.OnStopRolling += DisableDiceRolling;

        DisableDiceRolling();
    }
    
    public void SetAnimatorUpdateMode(bool unscaled)
    {
        if (rollingDiceAnimator == null)
            return;

        if (!unscaled)
            rollingDiceAnimator.updateMode = AnimatorUpdateMode.Normal;
        else
            rollingDiceAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void OnDestroy()
    {
        if (diceScript == null)
            return;

        diceScript.OnStartRolling -= OnStartRolling;
        diceScript.OnHitZoneEnter -= OnHitZoneEnter;
        diceScript.OnFinishRolling -= OnFinishRolling;
        diceScript.OnStopRolling -= DisableDiceRolling;
    }

    public void DisableDiceRolling()
    {
        buffImg.gameObject.SetActive(false);
        diceImg.gameObject.SetActive(false);
        mouseHint.gameObject.SetActive(false);
    }

    public void OnHitZoneEnter()
    {
        diceImg.color = timedColorMod;
        mouseHint.gameObject.SetActive(true);
    }

    public void OnStartRolling()
    {
        diceImg.gameObject.SetActive(true);
        diceImg.color = originalDiceColor;
    }

    public void OnFinishRolling(BuffInfo buffInfo)
    {
        if (buffInfo.isBuff)
        {
            diceImg.color = timeHitColor;
        }
        else
        {
            diceImg.color = untimedColorMod;
        }


        mouseHint.gameObject.SetActive(false);

        this.Co_DelayedExecute(() => RollResult(buffInfo.time -  0.4f, buffInfo.type), 0.4f, false);
    }

    void RollResult(float time, int type)
    {
        currentBuff = buffSprites[type];
        if (currentBuff.clip != null)
            src.PlayOneShot(currentBuff.clip);

        diceImg.gameObject.SetActive(false);
        buffImg.gameObject.SetActive(true);
        buffImg.sprite = currentBuff.sprite;

        Invoke(nameof(DisableBuff), time);
    }

    public void OnDiceSwitch(int newType)
    {
        currentBuff = buffSprites[newType];

        buffImg.sprite = currentBuff.sprite;
    }

    void DisableBuff()
    {
        buffImg.gameObject.SetActive(false);
    }


    [System.Serializable]
    class Buff
    {
        public Sprite sprite;
        public AudioClip clip;
    }
}