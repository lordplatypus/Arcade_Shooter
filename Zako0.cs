using System;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    public class Zako0 : Enemy
    {
        float count = 0;
        int bulletCount = 0;

        public Zako0(Game game, float x, float y)
            : base(game, x, y)
        {
            life = 5;
        }

        public override void Update()
        {
            x -= 1f;
            y -= (float)Math.Sin(count) * 10;
            count += .1f;

            bulletCount++;
            if (bulletCount % 30 == 0)
            {
                float angle = MyMath.PointToPointAngle(x, y, game.player.x, game.player.y);
                game.enemyBullets.Add(new EnemyBullet(x, y, angle, 3f));
            }
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, 0, Image.zako0);
        }
    }
}
