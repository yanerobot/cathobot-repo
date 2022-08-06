using UnityEngine;
using UnityEngine.UI;

public class UIReloadSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] UIBehaiv ui;

    EquipmentSystem es;
    bool canFill;

    void Start()
    {
        es = ui.GetPlayerHealth().GetComponent<EquipmentSystem>();
        es.OnReloadStart += StartReload;
    }

    public void StartReload(float time)
    {
        slider.gameObject.SetActive(true);
        slider.value = 0;
        slider.maxValue = time;

        canFill = true;
    }

    void Update()
    {
        if (!canFill)
            return;

        slider.value += Time.deltaTime;

        if (slider.value == slider.maxValue)
        {
            canFill = false;
            slider.gameObject.SetActive(false);
        }
    }
}
