using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(-1)]
public class UI_InitialGameLoader : MonoBehaviour
{
    [SerializeField] Button LevelSelectorGO;
    [SerializeField] TextMeshProUGUI newGameButtonTextObj;

    public static bool isContinue;


    void Awake()
    {
        LevelSelectorGO.interactable = false;
        if (PlayerPrefs.GetString(NameInputController.NAME_KEY, "") != "")
        {
            isContinue = true;
            newGameButtonTextObj.text = "CONTINUE";
        }

        if (PlayerPrefs.GetInt(ExitScript.PrefsKey, -1) > 0)
        {
            LevelSelectorGO.interactable = true;
        }
    }
}
