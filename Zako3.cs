using System;
using DxLibDLL;
using MyLib;


namespace Shooting
{
    public class Zako3 : Enemy
    {
        int count = 0;
        float angleToPlayer;

        public Zako3(Game game, float x, float y, int HPMultiplier)
            : base(game, x, y)
        {
            life = 10 * HPMultiplier;
        }

        public override void Update()
        {
            angleToPlayer = MyMath.PointToPointAngle(x, y, game.player.x, game.player.y);
            float speed = 2f;
            x += (float)Math.Cos(angleToPlayer) * speed;
            y += (float)Math.Sin(angleToPlayer) * speed;

            count++;
            if (count % 50 == 0)
            {
                game.enemyBullets.Add(new EnemyBullet(x, y, angleToPlayer, 8f));
            }
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, angleToPlayer, Image.zako3);
        }
    }
}