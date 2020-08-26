using System;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    public class Player
    {
        enum BulletState
        {
            Stright,
            Spread,
            missile,
        }
        //Player variables
        const float MoveSpeed = 6f;
        const int MutekiJikan = 120;
        float shootingAngle;
        public float x;
        public float y;
        public float collisionRadius = 4f;
        public bool isDead = false;
        float faceDirection = 0;
        public int life = 5 ;
        int mutekiTimer = 0;

        Game game;
        Mouse mouse;

        BulletState bulletState;

        //Variables that decide powerup level
        int strightBulletPowerUpCounter = 0;
        int spreadBulletPowerUpCounter = 0;
        int missileBulletPowerUpCounter = 0;

        public Player(float x, float y, Game game, Mouse mouse)
        {
            this.x = x;
            this.y = y;
            this.game = game;
            this.mouse = mouse;
            bulletState = BulletState.Stright;
        }

        public void Update()
        {
            float vx = 0f;
            float vy = 0f;

            if (Input.GetButton(DX.PAD_INPUT_4)) //left
            {
                vx = -MoveSpeed;
                faceDirection = 180 * MyMath.Deg2Rad;
            }
            else if (Input.GetButton(DX.PAD_INPUT_6)) //right
            {
                vx = MoveSpeed;
                faceDirection = 0;
            }
            if (Input.GetButton(DX.PAD_INPUT_8)) //up
            {
                vy = -MoveSpeed;
                faceDirection = 270 * MyMath.Deg2Rad;
            }
            else if (Input.GetButton(DX.PAD_INPUT_5)) //down
            {
                vy = MoveSpeed;
                faceDirection = 90 * MyMath.Deg2Rad;
            }

            if (vx != 0 && vy != 0)
            {
                vx /= MyMath.Sqrt2;
                vy /= MyMath.Sqrt2;
            }

            x += vx;
            y += vy;

            //bounderies
            if (x < 0) x = 0;
            if (x > Screen.Width) x = Screen.Width;
            if (y < 0) y = 0;
            if (y > Screen.Height) y = Screen.Height;

            shootingAngle = mouse.AngleToMouse(x, y);

            if (Input.GetMouseDown(DX.MOUSE_INPUT_LEFT) || Input.GetMouse(DX.MOUSE_INPUT_RIGHT))
            {
                if (bulletState == BulletState.Stright) Stright();
                if (bulletState == BulletState.Spread) Spread();
                if (bulletState == BulletState.missile) missile();
            }

            mutekiTimer--;
        }

        public void Draw()
        {
            if (mutekiTimer <= 0 || mutekiTimer % 2 == 0)
            {
                DX.DrawRotaGraphF(x, y, 1, faceDirection, Image.player);
                if (bulletState == BulletState.Stright) DX.DrawRotaGraphF(x, y, 1, shootingAngle, Image.gunStright);
                if (bulletState == BulletState.Spread) DX.DrawRotaGraphF(x, y, 1, shootingAngle, Image.gunSpread);
                if (bulletState == BulletState.missile) DX.DrawRotaGraphF(x, y, 1, shootingAngle, Image.gunMissile);
            }
        }

        public void OnCollisionEnemy(Enemy enemy)
        {
            if (mutekiTimer <= 0)
            {
                TakeDamage();
            }
        }

        public void OnCollisionEnemyBullet(EnemyBullet enemyBullet)
        {
            if (mutekiTimer <= 0)
            {
                TakeDamage();
            }
        }

        public void OnCollisionItem(Item item) //when the player picks up an item
        {
            if (item is HealthItem) life++; //increases health by 1
            if (item is StrightBulletPowerUpItem) //power up stright bullet
            {
                strightBulletPowerUpCounter++;
                bulletState = BulletState.Stright; //tells the computer to use stright bullets
                spreadBulletPowerUpCounter = 0; //resets power ups in other bullet types
                missileBulletPowerUpCounter = 0;
            }
            if (item is SpreadBulletPowerUpItem)
            {
                spreadBulletPowerUpCounter++;
                bulletState = BulletState.Spread;
                strightBulletPowerUpCounter = 0;
                missileBulletPowerUpCounter = 0;
            }
            if (item is MissileBulletPowerUpItem)
            {
                missileBulletPowerUpCounter++;
                bulletState = BulletState.missile;
                strightBulletPowerUpCounter = 0;
                spreadBulletPowerUpCounter = 0;
            }
        }

        public void OnCollisionHealthItem(HealthItem healthItem)
        {
            life++;
        }

        void TakeDamage()
        {
            Sound.Play(Sound.oof);
            life -= 1;
            if (life == 0)
            {
                isDead = true;
                game.explosions.Add(new Explosion(x, y));
            }
            else mutekiTimer = MutekiJikan;
        }

        void Stright()
        {
            game.playerBullets.Add(new StrightBullet(x, y, shootingAngle));

            float perpendicular = shootingAngle + 90 * MyMath.Deg2Rad; //set angle 90 degrees from mouse location

            for (int i = 1; i < strightBulletPowerUpCounter + 1; i++)
            {
                game.playerBullets.Add(new StrightBullet(x + i * 20 * (float)Math.Cos(perpendicular), y + i * 20 * (float)Math.Sin(perpendicular), shootingAngle));
                game.playerBullets.Add(new StrightBullet(x - i * 20 * (float)Math.Cos(perpendicular), y - i * 20 * (float)Math.Sin(perpendicular), shootingAngle));
            }
        }

        void Spread()
        {
            game.playerBullets.Add(new SpreadBullet(x, y, shootingAngle));

            if (spreadBulletPowerUpCounter > 0)
            {
                for (int i = 1; i < spreadBulletPowerUpCounter + 1; i++)
                {
                    game.playerBullets.Add(new SpreadBullet(x, y, i * -5f * MyMath.Deg2Rad + shootingAngle));
                    game.playerBullets.Add(new SpreadBullet(x, y, i * 5f * MyMath.Deg2Rad + shootingAngle));
                }
            }
        }

        void missile()
        {
            for (int i = 1; i < missileBulletPowerUpCounter + 1; i++)
            {
                game.playerBullets.Add(new MissileBullet(x, y, MyRandom.Range(0, 360) * MyMath.Deg2Rad, game));
            }            
        }
    }
}
