using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitScript : MonoBehaviour
{
    [SerializeField] BoxCollider2D coll;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject arrow;
    [SerializeField] TextMeshProUGUI textObject;
    [SerializeField] AudioSource src;
    
    AIManager aiManager;

    bool finished;
    bool enemiesDead;


    void Awake()
    {
        aiManager = FindObjectOfType<AIManager>();
    }

    void Update()
    {
        if (enemiesDead)
            return;

        if (aiManager == null || aiManager.transform.childCount == 0)
        {
            enemiesDead = true;
            OpenExit();
        }
        else
        {
            DisplayCurrentEnemies();
        }
    }
    
    void DisplayCurrentEnemies()
    {
        textObject.text = "ENEMIES: \n" + aiManager.transform.childCount;
    }

    void OpenExit()
    {
        textObject.gameObject.SetActive(false);
        coll.isTrigger = true;
        sr.color = Color.green;
        arrow.gameObject.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (finished)
            return;

        if (!collision.TryGetComponent(out TopDownMovement _))
            return;

        finished = true;

        var egs = GameObject.FindWithTag("UI");
        if (egs!= null)
        {
            egs.GetComponent<UIBehaiv>().EnableLevelCompletedUI();
        }        
    }
}
