using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EquipmentSystem : MonoBehaviour, IFreezible, IBuffable
{
    [SerializeField] internal Transform itemHolder;
    [SerializeField] float pickCooldown;

    [HideInInspector]
    public string holder;

    protected List<Item> itemsToPick;
    protected Item currentItem;

    float currentPickCD;

    Vector3 mouseDir;

    public static bool LevelEnded;

    public float Modifier { get; set; } = 1;

    void Awake()
    {
        LevelEnded = false;
        currentPickCD = pickCooldown;
    }

    void Update()
    {
        if (LevelEnded)
        {
            StopUsing();
            return;
        }

        currentPickCD += Time.unscaledDeltaTime;

        if (Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.E) && currentPickCD >= pickCooldown)
        {
            if (Equip())
                currentPickCD = 0;
        }
        if (Input.GetKeyDown(KeyCode.G))
            Toss();
        if (Input.GetMouseButtonDown(0))
            Use();
        if (Input.GetMouseButtonUp(0))
            StopUsing();
    }

    protected virtual void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        itemsToPick = new List<Item>();
    }
    public virtual bool Equip()
    {
        if (itemsToPick.Count == 0)
        {
            Debug.Log("Action cannot be performed! Item is not set!");
            return false;
        }
        if (currentItem != null)
        {
            Toss();
        }

        currentItem = GetBestItem();
        currentItem.WasEquippedBy(this);

        return true;
    }

    public virtual void Toss()
    {
        if (currentItem == null)
        {
            Debug.Log("No item equipped!");
            return;
        }
        currentItem.WasTossedAway();
        currentItem = null;
    }
    public void SetItem(Item item)
    {
        if (item == null)
            return;

        OnSetItem(item);
        itemsToPick.Add(item);
    }

    public void UnsetItem(Item item)
    {
        if (!itemsToPick.Contains(item))
            throw new System.DataMisalignedException($"{itemsToPick} doesn't contain an item: {item}. How it wasn't added to the list?");

        OnUnsetItem(item);
        itemsToPick.Remove(item);
    }

    public void Use()
    {
        if (currentItem == null)
            return;

        currentItem.Use();
    }

    public void StopUsing()
    {
        if (currentItem == null)
            return;

        currentItem.StopUsing();
    }

    public Item GetBestItem()
    {
        Item bestItem = null;
        float bestScore = -Mathf.Infinity;
        foreach (var item in itemsToPick)
        {
            var dist = Vector3.Distance(transform.position, item.transform.position);
            var dir = (item.transform.position - transform.position).normalized;
            var dot = Vector3.Dot(transform.forward, dir);

            var currentScore = 1 / dist * dot;
            if (currentScore > bestScore)
            {
                bestItem = item;
                bestScore = currentScore;
            }
        }

        return bestItem;
    }

    public void Buff(float newModifier, float time)
    {
        Modifier = newModifier;
        Invoke(nameof(SetNormalModifier), time);
    }

    public void SetNormalModifier()
    {
        Modifier = 1;
    }

    protected virtual void OnSetItem(Item item) { }
    protected virtual void OnUnsetItem(Item item) { }

    public virtual void Freeze() { }
    public virtual void UnFreeze() { }

    public virtual void DisableItemPick() { }
    public virtual void EnableItemPick() { }
}