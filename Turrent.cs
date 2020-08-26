using System;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    class Turrent : Enemy
    {
        float count = 0;
        float angle;
        float angleToPlayer;
        Boss2 boss;
        float angleV = 2f * MyMath.Deg2Rad;
        int gunID;

        public Turrent(Game game, float x, float y, float angle, Boss2 boss, int gunID, int HPMultiplier)
            : base(game, x, y)
        {
            life = 200 * HPMultiplier;
            this.angle = angle;
            this.boss = boss;
            this.gunID = gunID; //random number between 0 and 3 - decides what type of shooting patteren the gun will have
        }

        public override void Update()
        {
            angle += angleV; //change angle every frame
            //circle boss with a distance of 50
            x = (float)Math.Cos(angle) * 50 + boss.x;
            y = (float)Math.Sin(angle) * 50 + boss.y;

            angleToPlayer = MyMath.PointToPointAngle(x, y, game.player.x, game.player.y);

            if (gunID == 0)
            {//shoots at player every 50 frames within an error of 15 degrees
                count++;
                if (count % 50 == 0)
                {
                    game.enemyBullets.Add(new EnemyBullet(x, y, MyRandom.PlusMinus(15 * MyMath.Deg2Rad) + angleToPlayer, 8f));
                }
            }
            else if (gunID == 1)
            {//every 100 frames, shoots a shotgun blast of 10 bullets with varying speeds at the player, whithin an error of 45 degrees
                count++;
                if (count % 100 == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        game.enemyBullets.Add(new EnemyBullet(x, y, MyRandom.PlusMinus(45 * MyMath.Deg2Rad) + angleToPlayer, MyRandom.Range(1,8)));
                    }
                }
            }
            else if (gunID == 2)
            {//every 10 frames shoots in a random direction
                count++;
                if (count % 10 == 0)
                {
                    game.enemyBullets.Add(new EnemyBullet(x, y, MyRandom.PlusMinus(180 * MyMath.Deg2Rad), 8f));
                }
            }
            else if (gunID == 3)
            {//for 10 frames shoo in a circular pattern, resets after 40 frames
                count++;
                if (count < 10)
                {
                    game.enemyBullets.Add(new EnemyBullet(x, y, count * 36 * MyMath.Deg2Rad, count));
                }
                if (count > 50) count = 0;
            }
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, angleToPlayer, Image.turrent);
        }
    }
}
