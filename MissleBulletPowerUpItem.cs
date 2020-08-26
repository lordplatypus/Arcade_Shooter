using DxLibDLL;

namespace Shooting
{
    class MissileBulletPowerUpItem : Item
    {
        public MissileBulletPowerUpItem(Enemy enemy) : base(enemy)
        {
        }

        public override void Draw()
        {
            if (deathCounter <= 400 || deathCounter % 2 == 0) DX.DrawGraphF(x, y, Image.missileItem);
        }
    }
}
