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

        AI.aiPath.canMove = true;

        ResetRigidbody();
    }

    public override void OnUpdate()
    {
        timePassed += AI.deltaTime;

        if (timePassed > 1.5f)
        {
            timePassed = 0;
            ResetRigidbody();
        }
    }

    public override void OnExit()
    {
        AI.aiPath.canMove = false;
    }

    void ResetRigidbody()
    {
        AI.rb.velocity *= 0;
        AI.rb.angularVelocity *= 0;
    }
}
