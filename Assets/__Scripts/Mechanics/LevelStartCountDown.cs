using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class LevelStartCountDown : MonoBehaviour
{
    public const string TAG = "SafeZone";
    
    public static UnityEvent OnCountDownEnd;
    public static UnityEvent<int> OnSecondPassed;

    void Awake()
    {
        OnCountDownEnd = new UnityEvent();
        OnSecondPassed = new UnityEvent<int>();
        StartCoroutine(CountDown(3));
    }

/*    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TopDownMovement.PLAYERTAG)
        {
            OnCountDownEnd?.Invoke();
            Destroy(gameObject);
        }
    }*/

    void OnDisable()
    {
        OnCountDownEnd?.RemoveAllListeners();
        OnSecondPassed?.RemoveAllListeners();
    }

    IEnumerator CountDown(int time)
    {
        for (int i = 0; i < time + 1; i++)
        {
            yield return new WaitForSeconds(.5f);
            OnSecondPassed?.Invoke(i);
        }

        OnCountDownEnd?.Invoke();
        Destroy(gameObject);
    }
}
