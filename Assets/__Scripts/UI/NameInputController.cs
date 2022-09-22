using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NameInputController : MonoBehaviour
{
    [SerializeField] string forbiddenCharacters;
    [SerializeField] float maxTextLength;
    [SerializeField] Color colorForRestrictedSubmition;
    [SerializeField] Image inputFieldImage;
    [SerializeField] public UnityEvent OnSubmitEvent;

    internal const string NAME_KEY = "PlayerNickname";

    bool isSelected;

    string nickName;

    bool canSubmit;

    bool submitted;
    public static bool isEnabled;

    Color initialColor;

    void Awake()
    {
        initialColor = inputFieldImage.color;
    }

    void OnEnable()
    {
        isEnabled = true;
    }
    void OnDisable()
    {
        isEnabled = false;
    }
    void Update()
    {
        if (isSelected)
        {
            if (Input.GetButtonDown("Submit"))
            {
                SubmitNickname();
            }
        }
    }

    public void OnPlayButtonPressed()
    {
        if (PlayerPrefs.GetString(NAME_KEY, "") != "")
        {
            OnSubmitEvent?.Invoke();
            return;
        }

        gameObject.SetActive(true);
    }

    public void SubmitNickname()
    {
        if (submitted)
            return;

        if (!canSubmit)
        {
            //Little shake animation
            return;
        }

        submitted = true;

        if (string.IsNullOrEmpty(nickName))
            nickName = "Unknown human being";
        PlayerPrefs.SetString(NAME_KEY, nickName);
        PlayerAuthentication.Initialize(nickName, () => OnSubmitEvent?.Invoke());
    }
    public void OnSelect()
    {
        isSelected = true;
    }

    public void OnDeselect()
    {
        isSelected = false;
    }

    public void OnValueChanged(string text)
    {
        if (text.Length > maxTextLength || text.ContainsSpecialChar(forbiddenCharacters))
        {
            canSubmit = false;
            inputFieldImage.color = colorForRestrictedSubmition;
            return;
        }

        inputFieldImage.color = initialColor;
        canSubmit = true;
        nickName = text;
    }
}
