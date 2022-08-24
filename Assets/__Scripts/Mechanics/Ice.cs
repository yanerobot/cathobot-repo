using UnityEngine;

public class Ice : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IIceBehaivior iceBehaivior))
            iceBehaivior.OnIceEnter();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IIceBehaivior iceBehaivior))
            iceBehaivior.OnIceExit();
    }
}
