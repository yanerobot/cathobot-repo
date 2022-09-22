using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

public class RollingDiceTutorial : MonoBehaviour
{
    [SerializeField] UnityEvent OnAnyHintStart;
    [SerializeField] UnityEvent OnAnyHintEnd;
    [SerializeField] UnityEvent[] OnStartRollingHintEnter;
    [SerializeField] UnityEvent OnStartRollingHintBeforeDisable;
    [SerializeField] UnityEvent[] OnHitZoneEnterHintEnter;
    [SerializeField] UnityEvent OnHitZoneEnterHintBeforeDisable;
    [SerializeField] UnityEvent[] OnBuffHintEnter;
    [SerializeField] UnityEvent OnBuffHintBeforeDisable;
    [SerializeField] UnityEvent[] OnDebuffHintEnter;
    [SerializeField] UnityEvent OnDebuffHintBeforeDisable;
    DiceScript dice;

    bool startShown, hitEnterShown, buffShown, debuffShown;

    bool canSlideToNext;

    bool initialized;
    void Awake()
    {
        if (PlayerPrefs.GetInt("DiceTutFinished", 0) == 1)
        {
            return;
        }

        dice = FindObjectOfType<DiceScript>();

        dice.OnStartRolling += OnStartRolling;
        dice.OnHitZoneEnter += OnHitZoneEnter;
        //dice.OnFinishRolling += OnFinishRolling;
    }

    void Update()
    {
        canSlideToNext = Input.GetMouseButtonDown(0);
    }

    void OnDestroy()
    {
        if (!initialized)
            return;

        dice.OnStartRolling -= OnStartRolling;
        dice.OnHitZoneEnter -= OnHitZoneEnter;
        //dice.OnFinishRolling -= OnFinishRolling;
    }

    public void SlideNext()
    {
        canSlideToNext = true;
    }

    void OnStartRolling()
    {
        if (startShown)
            return;

        this.Co_DelayedExecute(() =>
        {

            startShown = true;
            StartCoroutine(ShowHint(OnStartRollingHintEnter, OnStartRollingHintBeforeDisable,
                DisablePredicate: () =>
                {
                    return Input.GetMouseButton(0);
                }));
        }, 0.1f);
    }

    void OnHitZoneEnter()
    {
        if (hitEnterShown)
            return;

        hitEnterShown = true;
        StartCoroutine(ShowHint(OnHitZoneEnterHintEnter, OnHitZoneEnterHintBeforeDisable, 
            DisablePredicate: () => {
                if (Input.GetMouseButtonDown(1))
                {
                    PlayerPrefs.SetInt("DiceTutFinished", 1);
                    return true;
                }
                return false;
            }));
    }

    void OnFinishRolling(BuffInfo buffInfo)
    {
        if (!buffInfo.isBuff)
        {
            if (debuffShown)
                return;

            debuffShown = true;
            StartCoroutine(ShowHint(OnDebuffHintEnter, OnDebuffHintBeforeDisable,
                DisablePredicate: () =>
                {
                    return Input.GetMouseButtonDown(0);
                }
                ));
        }
        else
        {
            if (buffShown)
                return;
            buffShown = true;

            StartCoroutine(ShowHint(OnBuffHintEnter, OnBuffHintBeforeDisable,
                DisablePredicate: () =>
                {
                    return Input.GetMouseButtonDown(1);
                }));
        }
    }

    IEnumerator ShowHint(UnityEvent[] EnableHint, UnityEvent BeforeDisable ,Func<bool> DisablePredicate)
    {
        OnAnyHintStart?.Invoke();
        Time.timeScale = 0;

        foreach (var hintEvent in EnableHint)
        {
            hintEvent?.Invoke();
            while (!canSlideToNext)
            {
                yield return null;
            }
            canSlideToNext = false;
        }

        while (DisablePredicate() == false)
        {
            yield return null;
        }
        BeforeDisable?.Invoke();
        Time.timeScale = 1;
    }
}
