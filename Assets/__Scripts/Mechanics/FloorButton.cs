using UnityEngine;

public class FloorButton : TriggerActivator
{
    [SerializeField] SpriteRenderer gfx;
    [SerializeField] Sprite pressedSprite;
    Sprite normalSprite;

    void Start()
    {
        normalSprite = gfx.sprite;
    }

    protected override void OnFirstOneEnter()
    {
        base.OnFirstOneEnter();
        gfx.sprite = pressedSprite;
    }

    protected override void OnLastOneExit()
    {
        base.OnLastOneExit();
        gfx.sprite = normalSprite;
    }
}
