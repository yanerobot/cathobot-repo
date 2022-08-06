using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] RectTransform eButtonTip;
    [SerializeField] Vector3 offset;
    [SerializeField] UIBehaiv ui;


    EquipmentSystem es;
    Camera cam;
    Item bestItem;

    void Start()
    {
        var player = ui.GetPlayerHealth();
        es = player.GetComponent<EquipmentSystem>();
        cam = Camera.main;
    }

    void Update()
    {
        if (es == null)
            return;

        bestItem = es.GetBestItem();

        if (bestItem == null)
        {
            eButtonTip.gameObject.SetActive(false);
            return;
        }

        eButtonTip.gameObject.SetActive(true);

        var wp = cam.WorldToScreenPoint(bestItem.transform.position + offset);
        eButtonTip.position = wp;
    }
}
