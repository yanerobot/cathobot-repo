using UnityEngine;

public class LevelHints : MonoBehaviour
{
    [SerializeField] GameObject[] toDisable;
    [SerializeField] GameObject[] toEnable;
    void Awake()
    {
        UIBehaiv.OnShowLevelHints += EnableHints;
        gameObject.SetActive(false);
    }

    void EnableHints()
    {
        gameObject.SetActive(true);
        foreach (var td in toDisable)
        {
            td.SetActive(false);
        }
        foreach (var te in toEnable)
        {
            te.SetActive(true);
        }
    }
}
