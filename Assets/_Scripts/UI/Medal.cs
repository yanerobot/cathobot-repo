using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Medal : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite gold, silver, bronze;
    [SerializeField] Color authorColor;
    [SerializeField] GameObject medalMessageGO;
    [SerializeField] GameObject authorMedalMessageGO;

    public static string CurrentMedalPrefs => "currentMedalPrefs" + SceneManager.GetActiveScene().buildIndex;

    void OnEnable()
    {
        var currentMedal = PlayerPrefs.GetInt(CurrentMedalPrefs, -1);

        if (currentMedal >= 0)
        {
            SetMedal((Type)currentMedal);
        }
    }

    public enum Type
    {
        Current, None, Bronze, Silver, Gold, Author
    }

    public void SetMedal(Type type)
    {
        print("Setting medal to: " + type);
        gameObject.SetActive(true);
        medalMessageGO.SetActive(false);
        authorMedalMessageGO.SetActive(false);

        image.color = Color.white;

        switch (type)
        {
            case Type.Author:
                image.sprite = gold;
                image.color = authorColor;
                authorMedalMessageGO.SetActive(true);
                break;
            case Type.Gold:
                image.sprite = gold;
                break;
            case Type.Silver:
                image.sprite = silver;
                break;
            case Type.Bronze:
                image.sprite = bronze;
                break;
            case Type.None:
                medalMessageGO.SetActive(true);
                image.sprite = gold;
                image.color = Color.black;
                break;
            case Type.Current:
                SetMedal((Type)PlayerPrefs.GetInt(CurrentMedalPrefs, -1));
                return;
        }
        
        PlayerPrefs.SetInt(CurrentMedalPrefs, (int)type);
    }
}
