using UnityEngine;

public class TurretCombatSystem : RangedCombatSystem
{
    [SerializeField] LineRenderer lineRendPrefab;
    [SerializeField] AudioClip preparationAudio;

    LineRenderer laserAimLineRend;

    public override void OnCombatStateExit()
    {
        base.OnCombatStateExit();
        if (laserAimLineRend != null)
            Destroy(laserAimLineRend.gameObject);
    }
    protected override void OnPrepareAttackStart()
    {
        src.PlayOneShot(preparationAudio);
        laserAimLineRend = Instantiate(lineRendPrefab, hitPoint.position, Quaternion.Euler(hitPoint.eulerAngles.AddTo(z: -90)), hitPoint);
    }
    protected override void OnPrepareAttackUpdate()
    {
        if (laserAimLineRend != null)
        {
            var hit = Physics2D.Raycast(hitPoint.position, hitPoint.right, 30, AI.visibleLayers);
            if (hit.collider != null)
            {
                laserAimLineRend.SetPosition(1, Vector3.zero.WhereY(hit.distance));
            }
        }
    }

    protected override void OnPrepareAttackEnd()
    {
        Destroy(laserAimLineRend.gameObject);
    }

    protected override Transform GetHitPoint()
    {
        return hitPoint;
    }
}
