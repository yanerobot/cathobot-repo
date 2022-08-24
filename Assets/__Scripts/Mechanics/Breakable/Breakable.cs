using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] Collider2D mainCollider;
    [SerializeField] SpriteRenderer GFXMain;
    [SerializeField] Sprite[] pieces;
    [SerializeField] BreackablePiece pieceGO;

    public void Break()
    {
        GFXMain.gameObject.SetActive(false);
        foreach (var sprite in pieces)
        {
            var breakablePiece = Instantiate(pieceGO, transform.position, Quaternion.identity, transform);
            breakablePiece.Init(sprite);
        }
        this.Co_DelayedExecute(() =>
        {
            mainCollider.enabled = false;
        }, 0.1f);
    }
}
