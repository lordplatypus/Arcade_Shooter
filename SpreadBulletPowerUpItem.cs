using DxLibDLL;

namespace Shooting
{
    class SpreadBulletPowerUpItem : Item
    {
        public SpreadBulletPowerUpItem(Enemy enemy) : base(enemy)
        {
        }

        public override void Draw()
        {
            if (deathCounter <= 400 || deathCounter % 2 == 0) DX.DrawGraphF(x, y, Image.spreadItem);
        }
    }
}
