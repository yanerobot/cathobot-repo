using UnityEngine;

public class Running : State
{
    EnemyAI AI;
    float timePassed;
    public Running(StateMachine fsm)
    {
        AI = fsm as EnemyAI;
    }

    public override void OnEnter()
    {
        if (AI.isStatic)
            return;

        if (!AI.movementDisabledExternally)
        {
            AI.aiPath.canMove = true;
            AI.ResetRigidbody();
        }
    }

    public override void OnUpdate()
    {
        timePassed += AI.deltaTime;

        if (timePassed > 1.5f)
        {
            timePassed = 0;
            AI.ResetRigidbody();
        }
    }

    public override void OnExit()
    {
        AI.aiPath.canMove = false;
    }
}
