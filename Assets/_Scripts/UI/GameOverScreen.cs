using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreenObj;

    Health playerHealth;
    void Start()
    {
        gameOverScreenObj.SetActive(false);
        var player = GameObject.FindWithTag(TopDownMovement.PLAYERTAG);

        playerHealth = player.GetComponent<Health>();
        playerHealth._OnDie.AddListener(ShowGameOverScreen);
    }

    void OnDestroy()
    {
        playerHealth?._OnDie.RemoveListener(ShowGameOverScreen);
    }

    void ShowGameOverScreen()
    {
        gameOverScreenObj.SetActive(true);
    }
}
