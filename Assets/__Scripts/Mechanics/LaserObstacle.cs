using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObstacle : Switcher
{
    [SerializeField] bool isActiveInitial;
    [SerializeField] SpriteRenderer laserSr;
    [SerializeField] Transform laserOrigin;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GraphUpdateScene gus;

    void Start()
    {
        SetActivation(isActiveInitial);

        if (!isActiveInitial)
            return;
        ProlongueLaser();
        this.Co_DelayedExecute(() => gus.Apply(), 1);
    }
    public override void Activation()
    {
        laserSr.gameObject.SetActive(IsActivated);
    }

    void FixedUpdate()
    {
        if (!isActiveInitial)
            return;
        ProlongueLaser();
    }

    void ProlongueLaser()
    {
        var hit = Physics2D.Raycast(laserOrigin.position, transform.right, 10, layerMask);

        if (hit.collider != null)
        {
            var initialSize = laserSr.size;
            laserSr.size = new Vector2(hit.distance, laserSr.size.y);
            var addedSize = laserSr.size - initialSize;
            laserSr.transform.position += transform.right * (addedSize.x / 2);
        }
    }
}
