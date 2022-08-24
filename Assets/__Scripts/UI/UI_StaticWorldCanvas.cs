using UnityEngine;

public class UI_StaticWorldCanvas : MonoBehaviour
{
    [SerializeField] bool fixRotation;
    [SerializeField] bool fixPosition;
    [SerializeField] bool fixScale;

    Vector3 initialPos, initialEulerAngles, initialScale;

    void Start()
    {
        initialPos = transform.localPosition;
        initialEulerAngles = transform.eulerAngles;
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (fixPosition)
            transform.position = transform.parent.position + initialPos;
        if (fixRotation)
            transform.eulerAngles = initialEulerAngles;
        if (fixScale)
            transform.localScale = initialScale.Divide(transform.parent.lossyScale);
    }
}
