using DxLibDLL;

namespace Shooting
{
    public class HealthItem : Item
    {
        public HealthItem(Enemy enemy) : base(enemy)
        {
        }

        public override void Draw()
        {
            if (deathCounter <= 400 || deathCounter % 2 == 0) DX.DrawGraphF(x, y, Image.healthItem);
        }
    }
}
