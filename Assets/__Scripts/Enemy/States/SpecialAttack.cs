using UnityEngine;

public class SpecialAttack : State
{
    EnemyAI AI;

    TopDownMovement movement;

    AI_SpecialAbility ability;

    public SpecialAttack(StateMachine fsm, AI_SpecialAbility ability)
    {
        AI = fsm as EnemyAI;
        this.ability = ability;
    }

    public override void OnEnter()
    {
        ability.OnSpecialEnter();
    }

    public override void OnUpdate()
    {
        ability.OnSpecialUpdate();
    }

    public override void OnExit()
    {
        ability.OnSpecialExit();

        foreach (var sa in AI.specialAbilities)
        {
            sa.component.ResetConditions();
        }
    }
}
