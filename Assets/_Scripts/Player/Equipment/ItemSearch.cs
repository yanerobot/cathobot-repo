using UnityEngine;
using System.Collections.Generic;

public class ItemSearch : MonoBehaviour
{
    public List<Item> items;
    EquipmentSystem equipmentSystem;

    void Awake()
    {
        equipmentSystem = GetComponentInParent<EquipmentSystem>();
    }

    void OnEnable()
    {
        items = new List<Item>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Item item))
        {
            equipmentSystem.SetItem(item);
            items.Add(item);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (items.Count == 0) return;

        foreach (var item in items)
        {
            if (collision.gameObject == item.gameObject)
            {
                equipmentSystem.UnsetItem(item);
                items.Remove(item);
                break;
            }
        }
    }
}
