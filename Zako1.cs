using System;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    public class Zako1 : Enemy
    {
        float count;

        public Zako1(Game game, float x, float y, float count)
            : base(game, x, y)
        {
            this.count = count;
            this.x = x;
            this.y = y;
            life = 10;
        }

        public override void Update()
        {
            if (count == 180)
            {
                x = MyRandom.Range(0, Screen.Width);
                y = MyRandom.Range(0, Screen.Height);
                for (int i = 0; i < 360; i += 10)
                {
                    game.enemyBullets.Add(new EnemyBullet(x, y, i * MyMath.Deg2Rad, 8f));
                }
                count = 0;
            }
            if (count == 30)
            {
                for (int i = 0; i < 360; i += 10)
                {
                    game.enemyBullets.Add(new EnemyBullet(x, y, i * MyMath.Deg2Rad, 6f));
                }
            }
            if (count == 60)
            {
                for (int i = 0; i < 360; i += 10)
                {
                    game.enemyBullets.Add(new EnemyBullet(x, y, i * MyMath.Deg2Rad, 4f));
                }
            }
            count++;
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, count * 4 * MyMath.Deg2Rad, Image.zako1);
        }
    }
}
