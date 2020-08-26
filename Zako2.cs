using System;
using DxLibDLL;
using MyLib;


namespace Shooting
{
    public class Zako2 : Enemy
    {
        const float Speed = 1f;
        float vx;
        float vy;

        public Zako2(Game game, float x, float y, float count)
            : base(game, x, y)
        {
            vx = ((float)Math.Cos(MyRandom.Range(90, 270) * MyMath.Deg2Rad)) * Speed;
            vy = ((float)Math.Sin(MyRandom.Range(90, 270) * MyMath.Deg2Rad)) * Speed;
            life = 10;
        }

        public override void Update()
        {
            x += vx;
            y += vy;

            if (y < 0 || y > Screen.Height)
            {
                vy = -vy;
            }

            game.enemyBullets.Add(new EnemyBullet(x, y, MyRandom.Range(0, 360) * MyMath.Deg2Rad, 8f));
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, 0, Image.zako2);
        }
    }
}