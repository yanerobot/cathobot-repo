using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombatSystem : EnemyCombatSystem
{
    [SerializeField] int attackDamage;
    [SerializeField] float attackRadius;
    [SerializeField] public AudioClip attackingAudio;
    [SerializeField] bool isSoundOnAttack;

    public float delayBetweenAttacks;

    AudioClip currentClip;

    public override void OnCombatStateEnter()
    {
        if (!isSoundOnAttack)
        {
            if (attackingAudio != null)
            {
                currentClip = AI.src.clip;
                AI.src.clip = AI.attackAudioClip;
                AI.src.Play();
            }
        }
    }

    public override void OnCombatStateUpdate()
    {

    }

    public override void OnCombatStateExit()
    {

    }
}
