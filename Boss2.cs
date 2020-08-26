using System;
using System.Collections.Generic;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    public class Boss2 : Enemy
    {
        public enum State
        {
            Appear,
            Normal,
            Swoon,
            Angry,
            Invincible,
            Dying,
        }

        public const int Size = 180;
        public const int CenterPointX = Screen.Width / 2;
        public const int CenterPointY = Screen.Height / 2;

        public State state = State.Appear;
        int swoonTime = 120;
        int dyingTime = 180;
        float angle;
        int specialAbilityCount = 0;
        float angleToPlayer = 180 * MyMath.Deg2Rad;
        bool mad = false;
        int turrentCount;
        int madCount = 0;
        float angleV = .5f * MyMath.Deg2Rad;
        int HPMultiplier;
        Wave wave;
        //made a new enemy list in this class as this allows the turrents and the boss they are attached to, to act independent of other bosses
        List<Turrent> turrents = new List<Turrent>();

        public Boss2(Game game, float x, float y, Wave wave, int HPMultiplier) : base(game, x, y)
        {
            life = 1500 * HPMultiplier;
            this.HPMultiplier = HPMultiplier;
            collisionRadius = 70;
            angle = MyMath.PointToPointAngle(x, y, CenterPointX, CenterPointY);
            for (int i = 0; i < 4; i++) //adds the four turrents to the boss
            {
                float turrentposition = 90 * i * MyMath.Deg2Rad;
                turrents.Add(new Turrent(game, x, y, turrentposition, this, MyRandom.Range(0,4), HPMultiplier));
                turrentCount = i;
            }
            this.wave = wave;
        }

        public override void Update()
        {
            if (state == State.Appear)
            {
                x += (float)Math.Cos(angle) * 5;
                y += (float)Math.Sin(angle) * 5;
                foreach (Turrent turrent in turrents) turrent.Update();

                if (Math.Abs(MyMath.DistanceBetweenTwoPoints(x, y, CenterPointX, CenterPointY)) < Screen.Width / 2 - Size)
                {//once the boss reaches a certain portion of the screen move into the main phase
                    state = State.Normal;
                }
            }
            else if (state == State.Normal)
            {
                angleToPlayer = MyMath.PointToPointAngle(x, y, game.player.x, game.player.y);

                foreach (Turrent turrent in turrents) turrent.Update();
                TurrentCollisionWithPlayerBullet(); //these are needed because these lists are in this class as opposed to the game class
                PointsForBlowingUpTurrent(); //award 50 points for blowing up a turrent
                turrents.RemoveAll(t => t.isDead); //remove dead turrents

                if (turrents.Count < turrentCount + 1)
                {
                    turrentCount--;
                    mad = true;
                }
                if (mad == true)
                {
                    madCount++;
                    angle += 4 * angleV; //move faster
                    if (madCount == 180)
                    {
                        angleV = -angleV; //reverse direction after "mad" phase
                        madCount = 0;
                        mad = false;
                    }
                }
                else angle += angleV; //default movement speed

                x = (float)Math.Cos(angle) * -MyMath.DistanceBetweenTwoPoints(CenterPointX, CenterPointY, x, y) + Screen.Width / 2;
                y = (float)Math.Sin(angle) * -MyMath.DistanceBetweenTwoPoints(CenterPointX, CenterPointY, x, y) + Screen.Height / 2;

                if (turrents.Count == 0) state = State.Swoon; //once turrents are destroyed go into swoon state
            }
            else if (state == State.Swoon)
            {//state bofore 2nd phase
                swoonTime--;

                if (swoonTime <= 0)
                {
                    state = State.Angry;
                }
            }
            else if (state == State.Angry)
            {//2nd phase
                angleToPlayer = MyMath.PointToPointAngle(x, y, game.player.x, game.player.y);

                if (specialAbilityCount == 0)
                {
                    for (int i = 0; i < 360; i += 20)
                    {
                        game.enemyBullets.Add(new EnemyBullet(x, y, i * MyMath.Deg2Rad, 8f));
                    }
                    for (int i = 10; i < 360; i += 20)
                    {
                        game.enemyBullets.Add(new EnemyBullet(x, y, i * MyMath.Deg2Rad, 4f));
                    }
                }
                if (specialAbilityCount > 100)
                {
                    if (specialAbilityCount % 2 == 0) x = x + specialAbilityCount - 100;
                    if (specialAbilityCount % 2 == 1) x = x - specialAbilityCount + 100;
                }

                if (specialAbilityCount == 200)
                {
                    specialAbilityCount = 0;
                    state = State.Invincible;
                }
                specialAbilityCount++;
            }
            else if (state == State.Invincible)
            {//this is when the boss teleports away - can't be hit in this state
                specialAbilityCount++;
                
                if (specialAbilityCount < 50)
                {
                    x = game.player.x;
                    y = game.player.y;
                }
                if (specialAbilityCount == 100)
                {
                    specialAbilityCount = 0;
                    state = State.Angry;
                }
            }
            else if (state == State.Dying)
            {//Death animation
                dyingTime--;
                y += .2f;
                angleToPlayer += .2f * MyMath.Deg2Rad;
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
            if (state == State.Appear)
            {
                DX.DrawRotaGraphF(x, y, 1, angle, Image.boss1);
                foreach (Turrent turrent in turrents) turrent.Draw();
            }
            else if(state == State.Normal)
            {
                DX.DrawRotaGraphF(x, y, 1, angleToPlayer, Image.boss1);
                foreach (Turrent turrent in turrents) turrent.Draw();
            }
            else if (state == State.Swoon)
            {
                if (swoonTime % 2 == 0) DX.DrawRotaGraphF(x, y, 1, angleToPlayer, Image.boss1);
                else DX.DrawRotaGraphF(x, y, 1, angleToPlayer, Image.boss3);
            }
            else if (state == State.Angry)
            {
                DX.DrawRotaGraphF(x, y, 1, angleToPlayer, Image.boss3);
            }
            else if (state == State.Invincible)
            {
                DX.DrawRotaGraphF(x, y, 1, 0, Image.teleportingSight);
            }
            else if (state == State.Dying)
            {
                DX.DrawRotaGraphF(x, y, 1, angleToPlayer, Image.boss3);
            }
        }

        public override void OnCollisionPlayerBullet(PlayerBullet playerBullet)
        {//boss takes damage
            if (state == State.Appear || state == State.Swoon || state == State.Dying) return;

            life -= 1;

            if (life <= 0)
            {
                state = State.Dying;
            }

            if (playerBullet is MissileBullet)
            {//extra sounds and effects if hit by a missile
                Sound.Play(Sound.explosion);
                game.explosions.Add(new Explosion(playerBullet.x, playerBullet.y));
            }
        }

        void TurrentCollisionWithPlayerBullet()
        {//detects if turrent takes damage
            foreach (PlayerBullet playerBullet in game.playerBullets)
            {
                if (playerBullet.isDead) continue;                

                foreach (Turrent turrent in turrents)
                {
                    if (turrent.isDead)continue;                   

                    if (MyMath.SphereSphereIntersection(
                        playerBullet.x, playerBullet.y, playerBullet.collisionRadius,
                        turrent.x, turrent.y, turrent.collisionRadius))
                    {
                        turrent.OnCollisionPlayerBullet(playerBullet);
                        playerBullet.OnCollisionEnemy(turrent);

                        if (playerBullet.isDead) break;
                    }
                }
            }
        }

        void PointsForBlowingUpTurrent()
        {//50 points for blowing up a turrent
            foreach (Turrent turrent in turrents)
            {
                if (turrent.isDead) game.score += 50;
            }
        }
    }
}
