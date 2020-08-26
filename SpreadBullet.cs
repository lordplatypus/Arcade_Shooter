using System;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    class SpreadBullet : PlayerBullet
    {
        public SpreadBullet(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Update()
        {
            x += (float)Math.Cos(angle) * Speed;
            y += (float)Math.Sin(angle) * Speed;

            if (x + VisibleRadius < 0 || x - VisibleRadius > Screen.Width ||
                y + VisibleRadius < 0 || y - VisibleRadius > Screen.Width)
            {
                isDead = true;
            }
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, angle, Image.spreadBullet);
        }
    }
}
