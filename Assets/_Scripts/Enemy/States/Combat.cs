using System.Collections;
using UnityEngine;

public class Combat : State
{
    EnemyAI AI;
    Coroutine attacking;
    AudioClip mainClip;

    public Combat(StateMachine fsm)
    {
        AI = fsm as EnemyAI;
    }

    public override void OnEnter()
    {
        attacking = AI.StartCoroutine(StartAttacking());

        if (!AI.isSoundOnAttack)
        {
            if (AI.attackAudioClip != null)
            {
                mainClip = AI.src.clip;
                AI.src.clip = AI.attackAudioClip;
                AI.src.Play();
            }
        }
    }

    public override void OnUpdate()
    {
        var dir = (Vector2)AI.target.position - (Vector2)AI.transform.position;

        if (AI.isStatic)
        {
            AI.staticTurret.transform.up = dir;
        }
        else
        {
            AI.transform.up = dir;
        }
    }

    public override void OnExit()
    {
        if (attacking != null)
            AI.StopCoroutine(attacking);

        if (mainClip != null)
        {
            AI.src.clip = mainClip;
            AI.src.Play();
        }
    }

    IEnumerator StartAttacking()
    {
        yield return new WaitForSeconds(AI.delayBetweenAttacks / 2);
        while (true)
        {
            if (AI.Attack())
                if (AI.isSoundOnAttack)
                    AI.src.PlayOneShot(AI.attackAudioClip);

            yield return new WaitForSeconds(AI.delayBetweenAttacks);
        }
    }
}
