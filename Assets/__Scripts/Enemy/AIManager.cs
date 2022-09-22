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

    [SerializeField] bool debugDisableEnemies;

    void Start()
    {
        LevelStartCountDown.OnCountDownEnd.AddListener(EnablePlayerFollow);

        if (debugDisableEnemies)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
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

    public List<Health> GetClosestEnemies(int count)
    {
        return transform.Cast<Transform>()
            .Select(t => t.GetComponent<EnemyAI>())
            .OrderBy(ai => ai.GetRunningDistance())
            .Take(count)
            .Select(t => t.GetComponent<Health>())
            .ToList();
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
