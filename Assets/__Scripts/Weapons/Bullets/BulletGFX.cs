using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGFX : MonoBehaviour
{
    [SerializeField] AnimationClip clip;
    Animator anim;
    TrailRenderer tr;

    void Awake()
    {
        tr = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
    }
    public float OnCollision()
    {
        float time = 0.01f;

        if (anim != null)
        {
            anim.SetTrigger("Destroyed");
            time = clip.length;
        }

        if (tr != null)
            time = Mathf.Max(time, tr.time);

        return time;
    }
}
