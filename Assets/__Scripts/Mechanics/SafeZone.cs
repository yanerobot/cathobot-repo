using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class SafeZone : MonoBehaviour
{
    public const string TAG = "SafeZone";
    
    public static UnityEvent OnSafeZoneExit;
    //public static UnityEvent<int> OnSecondPassed;


    void Awake()
    {
        OnSafeZoneExit = new UnityEvent();
        //OnSecondPassed = new UnityEvent<int>();
        //StartCoroutine(CountDown(3));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TopDownMovement.PLAYERTAG)
        {
            OnSafeZoneExit?.Invoke();
            Destroy(gameObject);
        }
    }

    void OnDisable()
    {
        OnSafeZoneExit?.RemoveAllListeners();
        //OnSecondPassed?.RemoveAllListeners();
    }

/*    IEnumerator CountDown(int time)
    {
        for (int i = 0; i < time + 1; i++)
        {
            yield return new WaitForSeconds(.5f);
            OnSecondPassed?.Invoke(i);
        }

        OnSafeZoneExit?.Invoke();
        Destroy(gameObject);
    }*/
}
