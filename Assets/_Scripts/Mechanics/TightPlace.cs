using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TightPlace : MonoBehaviour
{
    Hitbox hitbox;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out hitbox))
        {
            hitbox.isTightPlace = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (hitbox == null)
            return;

        if (collision.transform == hitbox.transform)
        {
            hitbox.isTightPlace = false;
        }
    }
}
