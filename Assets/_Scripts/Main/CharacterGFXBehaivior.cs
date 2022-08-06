using System.Collections;
using UnityEngine;

public class CharacterGFXBehaivior : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Color damagedColor;
    [SerializeField] float timeToBlink;
    [SerializeField] float delayBetweenBlinks;

    Color normalColor;
    Color initialDamagedColor;
    Color initalNormalColor;
    void Start()
    {
        normalColor = sr.color;
    }

    public float Blink()
    {
        StopAllCoroutines();
        StartCoroutine(Co_Blink());
        return timeToBlink;
    }

    public void TiltTickColor(Color color)
    {
        initialDamagedColor = damagedColor;
        initalNormalColor = normalColor;
        damagedColor *= color;
        normalColor *= color;
    }

    public void ResetDamagedColor()
    {
        damagedColor = initialDamagedColor;
        normalColor = initalNormalColor;
    }

    public IEnumerator Co_Blink()
    {
        var currentTime = 0f;
        var currentColor = 0;
        while(currentTime < timeToBlink)
        {
            if (currentColor == 0)
            {
                sr.color = damagedColor;
                currentColor = 1;
            }
            else
            {
                sr.color = normalColor;
                currentColor = 0;
            }

            yield return new WaitForSeconds(delayBetweenBlinks);
            currentTime += delayBetweenBlinks;
        }

        sr.color = normalColor;
    }
}
