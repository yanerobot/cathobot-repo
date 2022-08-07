using UnityEngine;

public class RunningCombat : State
{
    EnemyAI AI;
    float timePassed;
    public RunningCombat(StateMachine fsm)
    {
        AI = fsm as EnemyAI;
    }

    public override void OnEnter()
    {
        if (!AI.movementDisabledExternally)
        {
            AI.aiPath.canMove = true;
            AI.ResetRigidbody();
        }

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

        timePassed += AI.deltaTime;

        if (timePassed > 1.5f)
        {
            timePassed = 0;
            AI.ResetRigidbody();
        }

        AI.combatSystem.OnCombatStateUpdate();
    }

    public override void OnExit()
    {
        AI.combatSystem.OnCombatStateExit();
        AI.aiPath.canMove = false;
    }
}
