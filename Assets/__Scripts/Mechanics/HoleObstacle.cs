using System.Collections;
using UnityEngine;

public class HoleObstacle : MonoBehaviour
{
    [SerializeField] float scaleAfterFall;
    [SerializeField] float timeToFall;
    [SerializeField] float rotateOnFallAngle;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        if (collision.TryGetComponent(out Health health))
        {
            if (health.tag == TopDownMovement.PLAYERTAG)
                UIBehaiv.LevelEnded = true;

            StartCoroutine(Fall(health));
        }
    }

    IEnumerator Fall(Health fallingHealth)
    {    
        float initalScale = fallingHealth.transform.localScale.x;

        float curTime = 0;
        float currentScale = 0;
        while (curTime < timeToFall)
        {
            curTime += Time.deltaTime;

            currentScale = Mathf.Lerp(initalScale, scaleAfterFall, curTime / timeToFall);

            fallingHealth.transform.Rotate(Vector3.forward, curTime);

            fallingHealth.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }
        fallingHealth.Kill(ignoreInvulnirability: true);
    }
}
