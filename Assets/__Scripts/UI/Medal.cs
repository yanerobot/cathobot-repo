using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Medal : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite gold, silver, bronze;
    [SerializeField] Color authorColor;
    [SerializeField] GameObject medalMessageGO;
    [SerializeField] GameObject authorMedalMessageGO;
    [SerializeField] AudioSource src;
    public enum Type
    {
         None, Bronze, Silver, Gold, Author
    }

    public static Type GetMedal(float time, MedalTimesSO medals)
    {
        if (time <= medals.AuthorTime)
            return Type.Author;
        else if (time <= medals.GoldTime)
            return Type.Gold;
        else if (time <= medals.SilverTime)
            return Type.Silver;
        else if (time <= medals.BronzeTime)
            return Type.Bronze;
        else
            return Type.None;
    }

    public void SetMedal(Type type, bool newMedal = false)
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
        }
        if (newMedal)
        {
            image.DOFade(0.4f, 0);
            image.DOFade(1f, 0.4f);
            transform.localScale *= 2;
            
            image.transform.DOScale(1, 0.3f).OnComplete(() => src.Play());
        }
    }
}
