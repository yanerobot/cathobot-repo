public class KnockOut : State
{
    EnemyAI AI;
        
    public KnockOut(StateMachine fsm)
    {
        AI = fsm as EnemyAI;
    }

    public override void OnEnter()
    {
        AI.PlayHitAnimation();
    }

    public override void OnExit()
    {
        
    }
}
