using UnityEngine;
using DG.Tweening;

public class SlidingDoor : Switcher
{
    [SerializeField] float timeToOpen;
    [SerializeField] Vector2 offset;
    Vector2 initialPos;

    void Start()
    {
        initialPos = transform.position;
    }
    public override void Activation()
    {
        if (IsActivated)
            transform.DOMove(initialPos + offset, timeToOpen);
        else
            transform.DOMove(initialPos, timeToOpen);
    }
}
