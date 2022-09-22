using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnlyValuableBuffsToggle : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    public static string TogglePrefsKey => "OVB_Toggle" + SceneManager.GetActiveScene().buildIndex;
    public static string EnabledPrefsKey => "OVB_Enabled" + SceneManager.GetActiveScene().buildIndex;

    void OnEnable()
    {
        LoadToggleValue();
    }

    public void EnableOVB()
    {
        if (RollingDiceUI.IsRolling == false)
            return;

        gameObject.SetActive(true);
        PlayerPrefs.SetInt(EnabledPrefsKey, 1);
    }

    public void Toggle(bool value)
    {
        if (value)
            PlayerPrefs.SetInt(TogglePrefsKey, 1);
        else
            PlayerPrefs.SetInt(TogglePrefsKey, 0);
    }

    void LoadToggleValue()
    {
        int value = PlayerPrefs.GetInt(TogglePrefsKey, 0);

        toggle.isOn = value == 1;
    }
}
