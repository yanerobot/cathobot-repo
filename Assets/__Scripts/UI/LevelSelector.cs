using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] Button[] buttons;

    public void OnEnable()
    {
        var lastSavedLevel = PlayerPrefs.GetInt(ExitScript.PrefsKey, -1);

        if (lastSavedLevel <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < lastSavedLevel)
                buttons[i].interactable = true;
            else
                buttons[i].interactable = false;
        }
    }
}
