using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
