using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] Vector2 holdingOffset;
    [SerializeField] GameObject pickableHint;

    protected Collider2D col;
    protected EquipmentSystem character;
    internal GameObject hintObject;

    public UnityAction<bool> OnEquipAction;
    bool isEquipped;
    public bool IsEquipped { get { return isEquipped; } 
        private set {
            isEquipped = value;
            OnEquipAction?.Invoke(isEquipped);
        } 
    }


    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    public virtual void WasEquippedBy(EquipmentSystem character)
    {
        this.character = character;
        col.enabled = false;
        transform.SetParent(character.itemHolder.transform);
        transform.localPosition = holdingOffset;
        transform.localEulerAngles = Vector2.zero;
        pickableHint.gameObject.SetActive(false);
        IsEquipped = true;
    }
    public virtual void WasTossedAway()
    {
        col.enabled = true;
        transform.SetParent(null);
        transform.position = character.transform.position.AddTo(y: -1);
        transform.rotation = Quaternion.identity;
        character = null;
        pickableHint.gameObject.SetActive(true);
        IsEquipped = false;
    }

    public virtual void Use() { }
    public virtual void StopUsing() { }

    public virtual void Aim(Vector2 aimDirection) { }
}
