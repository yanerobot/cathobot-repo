using UnityEngine;
using System;

public abstract class AI_SpecialAbility : MonoBehaviour
{
    [SerializeField] protected EnemyAI AI;

    public virtual void OnSpecialEnter() { }
    public virtual void OnSpecialUpdate() { }
    public virtual void OnSpecialExit() { }

    public abstract bool EnterCondition();
    public abstract bool ExitCondition();
    public abstract void ResetConditions();
    public abstract void InitiateConditions();
}

[Serializable]
public class SpecialAbilityData
{
    public AI_SpecialAbility component;
    [Header("Can transition from:")]
    public bool running;
    public bool combat;
    public bool runningCombat;
}
