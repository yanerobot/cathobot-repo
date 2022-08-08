using UnityEngine;

public class Button : TriggerActivator
{
    [SerializeField] SpriteRenderer gfx;
    [SerializeField] Color pressedColor;
    Color normalColor;

    void Start()
    {
        normalColor = gfx.color;
    }

    protected override void OnFirstOneEnter()
    {
        base.OnFirstOneEnter();
        gfx.color = pressedColor;
    }

    protected override void OnLastOneExit()
    {
        base.OnLastOneExit();
        gfx.color = normalColor;
    }
}
