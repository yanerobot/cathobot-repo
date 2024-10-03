using LootLocker.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Collections;
using LootLocker;
using System.Linq;

public class LeaderBoardController : MonoBehaviour
{
    [SerializeField] GameObject loadingScreenGO;
    [SerializeField] NameInputController nameInput;
    [SerializeField] Transform leaderBoardParent;
    [SerializeField] ScoreDisplay scorePrefab;
    
    string leaderBoardKey => "best_times_stage_" + SceneManager.GetActiveScene().buildIndex;

    List<PlayerInfo> leaderBoardMembersToShow;
    bool top5Completed, last5Completed;

    string memberID;

    int? playerRank;
    string playerName;

    void Start()
    {
        top5Completed = false;
        last5Completed = false;

        LootLockerSDKManager.StartGuestSession(PlayerAuthentication.GetPlayerID(), response =>
        {
            if (response.success)
            {
                memberID = response.player_id.ToString();
                
                LootLockerSDKManager.GetPlayerName(response =>
                {
                    if (!response.success)
                    {
                        Debug.LogError(response.Error);
                    }

                    playerName = response.name;
                });
            }
            else
            {
                Debug.LogError("Could not connect to the leaderboard. " + response.Error);
            }
        });
    }

    public void SubmitHighScore(HighScoreData data)
    {
        LootLockerSDKManager.SubmitScore(memberID, Mathf.FloorToInt(data.time * 1000), leaderBoardKey, response =>
        {
            if (response.success)
            {
                Debug.Log("Succesfully submitted score: " + Mathf.FloorToInt(data.time * 1000) + ". Time is: " + data.time);
                if (string.IsNullOrEmpty(playerName))
                {
                    nameInput.gameObject.SetActive(true);
                    nameInput.OnSubmitEvent.AddListener(LoadHighScores);
                    return;
                }
                LoadHighScores();
            }
            else
            {
                Debug.Log("Failed to submit score. " + response.Error);
            }
        });
    }

    public void LoadHighScores()
    {
        leaderBoardMembersToShow = new List<PlayerInfo>();
        StartCoroutine(DisplayOnLoad());

        LootLockerSDKManager.GetScoreList(leaderBoardKey, 4, response =>
        {
            if (response.success == false)
            {
                print("error in first 5");
                return;
            }

            AddItemsToList(response.items);
            top5Completed = true;

            LootLockerSDKManager.GetMemberRank(leaderBoardKey, memberID, response =>
            {
                playerRank = response.rank;                

                if (!response.success || playerRank <= 9 || playerRank == 0)
                {
                    LootLockerSDKManager.GetScoreList(leaderBoardKey, 5, 4, response =>
                    {
                        if (response.success == false)
                        {
                            print("error in last 5");
                            //show error message
                            return;
                        }
                        
                        AddItemsToList(response.items);
                        last5Completed = true;
                    });
                }
                else
                {
                    LootLockerSDKManager.GetScoreList(leaderBoardKey, 7, (int)playerRank - 5, response =>
                    {
                        if (response.success == false)
                        {
                            print("error in player adjacent");
                            //show error
                            return;
                        }
                        LootLockerLeaderboardMember[] items;
                        items = new LootLockerLeaderboardMember[5];

                        if (response.items.Length == 7)
                        {
                            for (int i = 2; i < 7; i++)
                            {
                                items[i - 2] = response.items[i];
                            }
                        }
                        else if (response.items.Length == 6)
                        {
                            for (int i = 1; i < 6; i++)
                            {
                                items[i - 1] = response.items[i];
                            }
                        }
                        else
                        {
                            items = response.items;
                        }

                        AddItemsToList(items);
                        last5Completed = true;
                    });
                }
            });
        });
    }

    IEnumerator DisplayOnLoad()
    {
        yield return new WaitUntil(() => top5Completed && last5Completed);

        Destroy(loadingScreenGO);
        foreach (var item in leaderBoardMembersToShow)
        {
            var scoreItem = Instantiate(scorePrefab, leaderBoardParent);
            scoreItem.SetText(item.rank, item.name, TimerScript.ConvertToTimer(((float)item.score)/1000));

            if (playerRank != null && item.rank == (int)playerRank)
            {
                scoreItem.SetColor(Color.yellow);
            }
        }
    }

    void AddItemsToList(LootLockerLeaderboardMember[] items)
    {
        if (items == null || items.Length == 0)
        {
            //Do something
            return;
        }
        foreach (var i in items)
        {
            leaderBoardMembersToShow.Add(new PlayerInfo(i.player.name, i.score, i.rank));
        }
    }

    struct PlayerInfo
    {
        public string name;
        public int score;
        public int rank;

        public PlayerInfo(string name, int score, int rank)
        {
            
            this.name = name;
            this.score = score;
            this.rank = rank;
        }
    }
}
