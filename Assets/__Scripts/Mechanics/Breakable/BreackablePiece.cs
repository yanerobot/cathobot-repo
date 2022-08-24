using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class BreackablePiece : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float fadeDestroyAfterSeconds;
    [SerializeField] float delayBeforeFading;

    void Start()
    {
        if (fadeDestroyAfterSeconds > 0)
        {
            this.Co_DelayedExecute(() =>
            {
                GetComponent<SpriteRenderer>().DOFade(0.1f, fadeDestroyAfterSeconds).OnComplete(() => Destroy(gameObject));
            }, delayBeforeFading);
        }
    }

    public void Init(Sprite sprite)
    {
        sr.sprite = sprite;
        gameObject.AddComponent<PolygonCollider2D>();
    }
}
