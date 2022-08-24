using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    [SerializeField] float time, modifier;
    [SerializeField] Collider2D coll;
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null)
            return;

        if (Vector2.Dot(collision.attachedRigidbody.velocity, transform.right) <= 0)
            return;

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
