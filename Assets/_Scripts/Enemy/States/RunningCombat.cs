using UnityEngine;

public class RunningCombat : State
{
    EnemyAI AI;

    public RunningCombat(StateMachine fsm)
    {
        AI = fsm as EnemyAI;
    }

    public override void OnEnter()
    {
        AI.aiPath.canMove = true;
        AI.combatSystem.OnCombatStateEnter();
    }

    public override void OnUpdate()
    {
        var dir = (Vector2)AI.target.position - (Vector2)AI.transform.position;

        if (AI.isStatic)
        {
            AI.gfx.transform.up = dir;
        }
        else
        {
            AI.transform.up = dir;
        }

        AI.combatSystem.OnCombatStateUpdate();
    }

    public override void OnExit()
    {
        AI.combatSystem.OnCombatStateExit();
        AI.aiPath.canMove = false;
    }
}
