using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    [SerializeField] float time, modifier;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TopDownMovement movement))
        {
            movement.Buff(modifier, time);
        }
        else if (collision.TryGetComponent(out EnemyAI ai))
        {
            ai.BuffSpeed(modifier, time);
        }
    }
}
