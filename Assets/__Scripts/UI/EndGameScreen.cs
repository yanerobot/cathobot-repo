using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] OnlyValuableBuffsToggle ovbToggleController;
    [SerializeField] LeaderBoardController leaderBoard;
    [SerializeField] TimerScript timer;
    [SerializeField] TextMeshProUGUI currentScoreTextObj;
    [SerializeField] TextMeshProUGUI highScoreTextObj;
    [SerializeField] TextMeshProUGUI nextMedalTextObj;
    [SerializeField] GameObject nextMedalMainObj;
    [SerializeField] GameObject newHighScoreLabel;
    [Header("Medal")]
    [SerializeField] Medal medal;
    [SerializeField] MedalTimesSO medals;
    [SerializeField] Button nextLevelButton;

    public static string HighScoreData_Path => GameManager.SAVE_FILES_DIRECTORY + "HighscoreData/";
    public static string Prefs_Key => "HighScoreData_" + SceneManager.GetActiveScene().name + "_BI_" + SceneManager.GetActiveScene().buildIndex;

    void OnEnable()
    {
        var timerInfo = timer.StopTimer();

        var newTime = timerInfo.Item1;
        var newTimeText = timerInfo.Item2;

        var loadedData = PlayerPrefs.GetString(Prefs_Key, "");
        Serializer.TryDeserializeString(loadedData, out HighScoreData highScoreData);

        //Serializer.TryLoad(out HighScoreData highScoreData, HighScoreData_FileName, HighScoreData_Path);

        if (newTime < 1)
        {
            //is valid time ?
            highScoreTextObj.text = "Error";
            currentScoreTextObj.text = "Error";
            SetMedal((Medal.Type)highScoreData.medal);
            this.Co_DelayedExecute(() => leaderBoard.LoadHighScores(), 1f);
            return;
        }


        var highScoreTime = highScoreData.time;

        highScoreTextObj.text = highScoreData.timeText;

        if (highScoreTime == default(float))
            highScoreTime = Mathf.Infinity;
        
        if (newTime < highScoreTime)
        {
            //High score!
            var newMedal = Medal.GetMedal(newTime, medals);
            var saveData = new HighScoreData(newTime, newTimeText, (int)newMedal);

            var saveStringData = Serializer.SerializeToString(saveData);
            PlayerPrefs.SetString(Prefs_Key, saveStringData); //Serializer.Save(saveData, HighScoreData_FileName, HighScoreData_Path);
            Ghost g = FindObjectOfType<Ghost>();
            g?.SaveGhost(true);

            highScoreTextObj.text = newTimeText;

            newHighScoreLabel.SetActive(true);

            SetMedal(newMedal, (int)newMedal > highScoreData.medal);

            leaderBoard.SubmitHighScore(saveData);
        }
        else
        {
            SetMedal((Medal.Type)highScoreData.medal);
            this.Co_DelayedExecute(() => leaderBoard.LoadHighScores(), 1f);
        }

        currentScoreTextObj.text = newTimeText;

        var diceObj = FindObjectOfType<Dice>();
        if (diceObj != null)
            diceObj.StopDice();
    }

    void SetMedal(Medal.Type type, bool isNew = false)
    {
        medal.SetMedal(type, isNew);
        SetNextMedalTime(type);
        ovbToggleController.gameObject.SetActive(false);

        if ((int)type >= (int)Medal.Type.Silver)
        {
            nextLevelButton.interactable = true;
            ovbToggleController.EnableOVB();
        }
    }

    void SetNextMedalTime(Medal.Type currentMedal)
    {
        var nextMedalTime = GetNewMedalTime(currentMedal);

        if (nextMedalTime == null)
            nextMedalMainObj.SetActive(false);
        else
            nextMedalTextObj.text = TimerScript.ConvertToTimer((float)nextMedalTime);
    }

    float? GetNewMedalTime(Medal.Type type)
    {
        switch (type)
        {
            case Medal.Type.None:
                return medals.BronzeTime;
            case Medal.Type.Bronze:
                return medals.SilverTime;
            case Medal.Type.Silver:
                return medals.GoldTime;
            case Medal.Type.Gold:
                return medals.AuthorTime;
        }

        return null;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
