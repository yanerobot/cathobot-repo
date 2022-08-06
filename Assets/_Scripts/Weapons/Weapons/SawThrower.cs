public class SawThrower : Weapon
{
    SawBullet sawBullet;
    bool hasBullets;
    
    public override void WasEquippedBy(EquipmentSystem character)
    {
        base.WasEquippedBy(character);
    }
    protected override void Shoot()
    {
        if (sawBullet == null && CurrentBullets > 0)
        {
            var bullet = SingleShot();

            if (bullet is SawBullet sawBullet)
                this.sawBullet = sawBullet;
        }
        else if (sawBullet != null)
        {
            CurrentBullets--;
            sawBullet.Explode();
            Destroy(sawBullet.gameObject);
            sawBullet = null;
        }
    }
}
