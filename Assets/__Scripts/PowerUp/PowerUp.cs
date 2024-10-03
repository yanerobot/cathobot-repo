using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour
{
    [SerializeField, Tooltip("Buffs:\n 0: Speed\n 2: Hitbox\n 4: Shield \n 5: ShootAround \n 7: Damage \n 9: Kill Enemies \n\n Debuffs: \n 1: Damage \n 3: Enemy Shields \n 6: Speed \n 8: Hitbox")] 
    float buffTime;
    [SerializeField] float debuffTime;
    [SerializeField] int[] increasedChanceBuffType;
    [SerializeField] List<NestedList<int>> valuableBuffs;
    [SerializeField] int[] decreasedChanceBuffType;
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

    [HideInInspector]
    public int currentBuff;

    AIManager aiManager;

    int currentPowerUpNum;

    void Start()
    {
        var enemyManagerGO = GameObject.FindWithTag(AIManager.TAG);
        if (enemyManagerGO != null)
        {
            aiManager = enemyManagerGO.GetComponent<AIManager>();
        }
    }

    public BuffInfo ChoosePowerUp(bool isBuff)
    {
        if (es == null || movement == null || hitbox == null)
        {
            Debug.LogWarning("Scripts are not attached");
            return null;
        }

        int buffType = Roll(isBuff);

        var buffTime = SetBuff(buffType);

        var buffInfo = new BuffInfo(buffTime, buffType, isBuff);

        currentBuff = buffType;

        currentPowerUpNum++;

        this.Co_DelayedExecute(() => currentBuff = -1, buffInfo.time);

        return buffInfo;
    }


    int Roll(bool isBuff)
    {
        int buffType = -1;

        int[] buffIndecies = new int[] { 0, 2, 4, 5, 7, 9 };

        int[] debuffIndecies = new int[] { 1, 3, 6, 8 };


        if (isBuff)
        {
            if (PlayerPrefs.GetInt(OnlyValuableBuffsToggle.TogglePrefsKey, 0) == 1 && valuableBuffs.Count > currentPowerUpNum)
            {
                var currentBuffs = valuableBuffs[currentPowerUpNum];

                buffType = currentBuffs.list[Random.Range(0, currentBuffs.list.Count)];

                //buffType = setExactBuffType[Random.Range(0, setExactBuffType.Length)];

                buffType = CheckChances(buffType, currentBuffs.list.ToArray());
                return buffType;
            }

            buffType = buffIndecies[Random.Range(0, buffIndecies.Length)];

            buffType = CheckChances(buffType, buffIndecies);
        }
        else
        {
            buffType = debuffIndecies[Random.Range(0, debuffIndecies.Length)];

            buffType = CheckChances(buffType, debuffIndecies);
        }

        return buffType;

        int CheckChances(int buffType, int[] array)
        {
            if (!Utils.DoesElementExistInArray(buffType, increasedChanceBuffType))
                buffType = array[Random.Range(0, array.Length)];

            if (Utils.DoesElementExistInArray(buffType, decreasedChanceBuffType))
                buffType = array[Random.Range(0, array.Length)];

            return buffType;
        }
    }



    float SetBuff(int buffType)
    {
        float buffTime = this.buffTime;
        if (buffType == 1 || buffType == 3 || buffType == 6 || buffType == 8) 
            buffTime = debuffTime;

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
                buffTime = bulletBuffTime;
                break;
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
                buffTime = 0.3f * enemiesToKill;
                break;
            default:
                buffTime = 0;
                break;
        }

        if (buffTime == 0)
            Debug.LogError("Time shouldnt be 0");

        return buffTime;
    }

    IEnumerator KillEnemies()
    {
        var enemies = aiManager.GetClosestEnemies(enemiesToKill);

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
[System.Serializable]
public class NestedList<T>
{
    public List<T> list;
}

public class BuffInfo
{
    public float time;
    public int type;
    public bool isBuff;

    public BuffInfo(float time, int type, bool isBuff)
    {
        this.time = time;
        this.type = type;
        this.isBuff = isBuff;
    }
}