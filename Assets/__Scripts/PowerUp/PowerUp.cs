using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float buffTime;
    [SerializeField] int increasedChanceBuff = -1;
    [SerializeField, Tooltip("Should be negative if not neededed")] int setExactBuffType;
    [Space]
    [Header("Speed")]
    [SerializeField] float speedBuffValue;
    [SerializeField] float speedDebuffValue;
    [Header("Damage")]
    [SerializeField] float damageBuffValue;
    [SerializeField] float damageDebuffValue;
    [Header("HitBox")]
    [SerializeField] float hitboxBuffValue;
    [SerializeField] float hitboxDebuffValue;
    [Header("Bullets Around")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bullets;
    [SerializeField] float spreadAngle;
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float shootingDelay;
    [SerializeField] float bulletBuffTime;
    [Header("Kill enemies")]
    [SerializeField] int enemiesToKill;


    //player components
    [Header("Player Components")]
    [SerializeField] EquipmentSystem es;
    [SerializeField] TopDownMovement movement;
    [SerializeField] Hitbox hitbox;
    [SerializeField] Health playerHealth;

    static int previousBuff = -1;
    static (bool, int) shouldSetIncreasedChanceBuff;

    [HideInInspector]
    public int currentBuff;

    AIManager aiManager;

    void Start()
    {
        var enemyManagerGO = GameObject.FindWithTag(AIManager.TAG);
        if (enemyManagerGO != null)
        {
            aiManager = enemyManagerGO.GetComponent<AIManager>();
        }
    }

    public (float, int) ChoosePowerUp(bool isBuff)
    {
        if (es == null || movement == null || hitbox == null)
        {
            Debug.LogWarning("Scripts are not attached");
            return (0, 0);
        }

        int buffType = Roll(isBuff);

        if (buffType == previousBuff)
            buffType = Roll(isBuff);

        previousBuff = buffType;


        if (increasedChanceBuff >= 0 && shouldSetIncreasedChanceBuff.Item1 && buffType != increasedChanceBuff)
        {
            buffType = increasedChanceBuff;
            shouldSetIncreasedChanceBuff.Item1 = false;
            shouldSetIncreasedChanceBuff.Item2 = 0;
        }
        else if (buffType == increasedChanceBuff)
        {
            shouldSetIncreasedChanceBuff.Item1 = false;
            shouldSetIncreasedChanceBuff.Item2 = 0;
        }
        else
        {
            shouldSetIncreasedChanceBuff.Item2++;
            if (shouldSetIncreasedChanceBuff.Item2 >= 3)
            {
                shouldSetIncreasedChanceBuff.Item1 = true;
            }
        }


        if (setExactBuffType >= 0)
            buffType = setExactBuffType;

        switch (buffType)
        {
            case 0:
                movement.Buff(speedBuffValue, buffTime);
                break;
            case 1:
                es.Buff(damageDebuffValue, buffTime);
                break;
            case 2:
                hitbox.Buff(hitboxBuffValue, buffTime);
                break;
            case 3:
                aiManager.MakeInvulnirable(3, buffTime);
                break;
            case 4:
                playerHealth.MakeInvulnirable(buffTime);
                break;
            case 5:
                StartCoroutine(ShootInEveryDirection());
                return (bulletBuffTime, buffType);
            case 6:
                movement.Buff(speedDebuffValue, buffTime);
                break;
            case 7:
                es.Buff(damageBuffValue, buffTime);
                break;
            case 8:
                hitbox.Buff(hitboxDebuffValue, buffTime);
                break;
            case 9:
                StartCoroutine(KillEnemies());
                return (1, buffType);
            default:
                return (-1, buffType);
        }

        currentBuff = buffType;

        if (buffType == 9)
            this.Co_DelayedExecute(() => currentBuff = -1, (float)enemiesToKill);
        else if (buffType == 5)
            this.Co_DelayedExecute(() => currentBuff = -1, bulletBuffTime);
        else
            this.Co_DelayedExecute(() => currentBuff = -1, buffTime);

        return (buffTime, buffType);
    }

    int Roll(bool isBuff)
    {
        int[] buffIndexs = new int[] { 0, 2, 4, 5, 7 };
        int killEnemiesBuff = 9;

        int[] debuffIndexs = new int[] { 1, 6, 8 };
        int enemyShieldDebuff = 3;


        if (isBuff)
        {
            if (Random.Range(0, 1f) < 0.02f)
                return killEnemiesBuff;
            else
                return buffIndexs[Random.Range(0, buffIndexs.Length)];
        }
        else
        {
            if (Random.Range(0, 9) == 1)
                return enemyShieldDebuff;
            else
                return debuffIndexs[Random.Range(0, debuffIndexs.Length)];
        }

    }

    IEnumerator KillEnemies()
    {
        var enemies = aiManager.GetClosestEnemies(transform.position, enemiesToKill);

        foreach (var enemy in enemies)
        {
            yield return new WaitForSeconds(0.3f);
            
            enemy.Kill();
        }
    }

    IEnumerator ShootInEveryDirection()
    {
        float curTime = 0;

        while(curTime < bulletBuffTime)
        {
            SpreadShot();
            yield return new WaitForSeconds(shootingDelay);
            curTime += shootingDelay;
        }
    }

    void SpreadShot()
    {
        GameObject bullet;
        Bullet bulletComp;
        var angle = spreadAngle / bullets;
        for (int i = 0; i < bullets; i++)
        {
            bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, null);
            bullet.transform.Rotate(Vector3.forward, (i - Mathf.FloorToInt(bullets * 0.5f)) * angle);
            bulletComp = bullet.GetComponent<Bullet>();
            bulletComp.Init(transform.parent.gameObject, damage, bulletSpeed, 1);
        }
    }
}