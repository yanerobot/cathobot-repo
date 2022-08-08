using UnityEngine;

public abstract class Activator : MonoBehaviour
{
    [SerializeField] Switcher[] switchers;
    protected void Activate(bool value)
    {
        foreach (var switcher in switchers)
        {
            switcher.SetActivation(value);
        }
    }

    protected void Toggle()
    {
        foreach (var switcher in switchers)
        {
            switcher.Toggle();
        }
    }
}
