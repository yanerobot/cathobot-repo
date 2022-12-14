using UnityEngine;

public abstract class Switcher : MonoBehaviour
{
    bool isActivated;
    public bool IsActivated
    {
        get { return isActivated; }
    }
    public void Toggle()
    {
        SetActivation(!isActivated);
    }

    public void SetActivation(bool value)
    {
        isActivated = value;
        Activation();
    }

    public abstract void Activation();
}
