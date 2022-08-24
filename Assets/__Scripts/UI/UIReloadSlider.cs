using UnityEngine;
using UnityEngine.UI;

public class UIReloadSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] EquipmentSystem es;
    bool canFill;

    void Start()
    {
        es.OnReloadStart += StartReload;
        slider.gameObject.SetActive(false);
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
