using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class AIManager : MonoBehaviour
{
    public const string TAG = "AIManager";
    public Health target;
    public Action onEnemiesDie;

    public bool canFollowPlayer = false;

    void Start()
    {
        SafeZone.OnSafeZoneOut.AddListener(EnablePlayerFollow);
    }

    void EnablePlayerFollow()
    {
        canFollowPlayer = true;
    }

    public Transform FindPlayer()
    {
        var player = GameObject.FindWithTag(TopDownMovement.PLAYERTAG);

        if (player != null)
            if (player.TryGetComponent(out target))
                return target.transform;
        
        return null;
    }

    public List<Health> GetClosestEnemies(Vector3 pos, int count)
    {
        List<Health> enemies = new List<Health>();

        Transform[] enemyTransforms = transform.Cast<Transform>().ToArray();

        enemies = enemyTransforms.OrderBy(t => (t.position - pos).sqrMagnitude)
            .Take(count)
            .Select(t => t.GetComponent<Health>())
            .ToList();

        return enemies;
    }

    public void KillClosestEnemy(Vector2 pos)
    {
        Transform closest = null;



        if (closest != null)
            closest.GetComponent<Health>().Kill();
    }

    public void MakeInvulnirable(int count, float time)
    {
        int ind = 0;
        foreach(Transform t in transform)
        {
            if (ind > count)
                break;

            t.GetComponent<Health>().MakeInvulnirable(time);
            ind++;
        }
    }
}
