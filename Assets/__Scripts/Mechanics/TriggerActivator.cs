using UnityEngine;

public class TriggerActivator : Activator
{

    [SerializeField] LayerMask ignoreLayers;

    int currentCollisions;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreLayers.Contains(collision.gameObject.layer))
            return;

        currentCollisions++;
        if (currentCollisions == 1)
        {
            OnFirstOneEnter();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (ignoreLayers.Contains(collision.gameObject.layer))
            return;

        currentCollisions--;
        if (currentCollisions == 0)
        {
            OnLastOneExit();
        }
    }

    protected virtual void OnFirstOneEnter() => Activate(true);
    protected virtual void OnLastOneExit() => Activate(false);
}
