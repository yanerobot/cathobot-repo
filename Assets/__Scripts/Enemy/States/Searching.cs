using UnityEngine;

public class Searching : State
{
    EnemyAI AI;
    Transform target;
    public Searching(StateMachine fsm)
    {
        AI = fsm as EnemyAI;
    }

    public override void OnEnter()
    {
        AI.SetUpdateMode(StateMachine.UpdateMode.Custom, 0.2f);
    }

    public override void OnUpdate()
    {
        if (AI.destinationSetter.target != null)
            return;

        target = AI.aiManager.FindPlayer();

        if (target != null)
            AI.SetTarget(target);
    }

    public override void OnExit()
    {
        AI.SetUpdateMode(default);
    }
}
