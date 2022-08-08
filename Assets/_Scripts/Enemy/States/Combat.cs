using UnityEngine;

public class Combat : State
{
    EnemyAI AI;

    TopDownMovement movement;

    public Combat(StateMachine fsm)
    {
        AI = fsm as EnemyAI;
    }

    public override void OnEnter()
    {
        AI.combatSystem.OnCombatStateEnter();
        movement = AI.target.GetComponent<TopDownMovement>();
    }

    public override void OnUpdate()
    {
        AI.combatSystem.OnCombatStateUpdate();
    }

    public override void OnExit()
    {
        AI.combatSystem.OnCombatStateExit();
    }
}
