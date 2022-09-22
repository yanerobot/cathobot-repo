using UnityEngine;

public class KillOnTouch : MonoBehaviour
{
    [SerializeField] float delay;
    bool instaKill;

    void OnEnable()
    {
        instaKill = true;
        this.Co_DelayedExecute(() => instaKill = false, delay);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            if (instaKill)
            {
                health.Kill();
                return;
            }
            this.Co_DelayedExecute(() => health.Kill(), delay);
        }
    }
}
