using System;
using UnityEngine;
using LootLocker.Requests;

public class PlayerAuthentication
{
    public static bool isInitialized;
    const string PLAYER_ID_KEY = "PlayerID";

    public static void Initialize(string name, Action callback)
    {
        LootLockerSDKManager.StartGuestSession(GetPlayerID(), response =>
        {
            if (response.success)
            {
                Debug.Log("Successfully initialized player");

                PlayerPrefs.SetString(PLAYER_ID_KEY, response.player_id.ToString());

                LootLockerSDKManager.SetPlayerName(name, response =>
                {
                    if (response.success)
                    {
                        Debug.Log("Successfully set player's name to " + name);
                        callback?.Invoke();
                    }
                    else
                        callback?.Invoke();
                });
            }
            else
            {
                Debug.LogError("Initialization failed: " + response.Error);
                callback?.Invoke();
            }
        });
    }

    public static string GetPlayerID()
    {
        var id = PlayerPrefs.GetString(PLAYER_ID_KEY, "");
        if (string.IsNullOrEmpty(id))
        {
            id = DateTime.Now.ToString("ddMMyyyyHHmmss") + UnityEngine.Random.Range(0, 10000).ToString() + SystemInfo.graphicsDeviceID;

            PlayerPrefs.SetString(PLAYER_ID_KEY, id);
        }

        return id;
    }
}
