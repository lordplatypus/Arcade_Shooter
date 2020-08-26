using DxLibDLL;
using MyLib;
using System;

namespace Shooting
{
    public class Boss : Enemy
    {
        enum State
        {
            Appear,
            Normal,
            Swoon,
            Angry,
            Dying,
        }

        public const int Size = 180;

        State state = State.Appear;
        int swoonTime = 120;
        int dyingTime = 180;
        float angle;
        float playerX;
        float playerY;
        int specialAbilityCount = 0;
        int bulletCount = 99;

        public Boss(Game game, float x, float y) : base(game, x, y)
        {
            life = 500;
            collisionRadius = 70;
        }

        public override void Update()
        {
            if (state == State.Appear)
            {
                x -= 5;

                if (x <= 750)
                {
                    state = State.Normal;
                }
            }
            else if (state == State.Normal)
            {
                if (playerY > y) y += 1;
                if (playerY < y) y -= 1;
                x += (float)Math.Cos((bulletCount * 3) * MyMath.Deg2Rad);
                y += (float)Math.Sin((bulletCount * 3) * MyMath.Deg2Rad);

                bulletCount++;
                if (bulletCount == 100)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        game.enemyBullets.Add(new EnemyBullet(x, y, MyRandom.Range(150, 210) * MyMath.Deg2Rad, MyRandom.Range(1f, 8f)));
                    }
                    bulletCount = 0;
                }

                if (playerX > x)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        game.enemyBullets.Add(new EnemyBullet(x, y, MyRandom.PlusMinus(45) * MyMath.Deg2Rad, MyRandom.Range(1f, 8f)));

                    }
                }
            }
            else if (state == State.Swoon)
            {
                swoonTime--;

                if (swoonTime <= 0)
                {
                    state = State.Angry;
                }
            }
            else if (state == State.Angry)
            {
                specialAbilityCount++;
                if (specialAbilityCount < 100)
                {
                    if (specialAbilityCount % 2 == 0) x = x + specialAbilityCount;
                    if (specialAbilityCount % 2 == 1) x = x - specialAbilityCount;
                }
                if (specialAbilityCount == 100) x = Screen.Width + 100;
                if (specialAbilityCount == 150)
                {
                    x = MyRandom.Range(0, Screen.Width);
                    y = MyRandom.Range(0, Screen.Height);
                    for (int i = 0; i < 360; i += 20)
                    {
                        game.enemyBullets.Add(new EnemyBullet(x, y, i * MyMath.Deg2Rad, 8f));
                    }
                    for (int i = 10; i < 360; i += 20)
                    {
                        game.enemyBullets.Add(new EnemyBullet(x, y, i * MyMath.Deg2Rad, 4f));
                    }
                }
                if (specialAbilityCount == 200) specialAbilityCount = 0;
            }
            else if (state == State.Dying)
            {
                dyingTime--;
                y += .2f;
                angle += .2f * MyMath.Deg2Rad;
                x = MyRandom.Range(x - 2f, x + 2f);

                if (dyingTime % 4 == 0)
                {
                    game.explosions.Add(new Explosion(
                        MyRandom.Range(x - Size / 2, x + Size / 2), 
                        MyRandom.Range(y - Size / 2, y + Size / 2)));
                }

                if (dyingTime <= 0)
                {
                    isDead = true;
                    for (int i = 0; i < 25; i++)
                    {
                        game.explosions.Add(new Explosion(
                            MyRandom.Range(x - Size / 2, x + Size / 2),
                            MyRandom.Range(y - Size / 2, y + Size / 2)));
                    }
                }
            }
        }

        public override void Draw()
        {
            if (state == State.Appear || state == State.Normal)
            {
                DX.DrawRotaGraphF(x, y, 1, 0, Image.boss1);
            }
            else if (state == State.Swoon)
            {
                DX.DrawRotaGraphF(x, y, 1, 0, Image.boss2);
            }
            else if (state == State.Angry)
            {
                DX.DrawRotaGraphF(x, y, 1, 0, Image.boss3);
            }
            else if (state == State.Dying)
            {
                DX.DrawRotaGraphF(x, y, 1, angle, Image.boss2);
            }
        }

        public override void OnCollisionPlayerBullet(PlayerBullet playerBullet)
        {
            if (state == State.Appear || state == State.Swoon || state == State.Dying) return;

            life -= 1;

            if (life <= 0)
            {
                state = State.Dying;
            }
            else if (state == State.Normal && life <= 300)
            {
                state = State.Swoon;
            }
        }

        public void PlayerLocation(Player player)
        {
            playerX = player.x;
            playerY = player.y;
        }
    }
}
