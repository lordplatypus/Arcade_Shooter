using System;
using System.Collections.Generic;
using DxLibDLL;
using MyLib;


namespace Shooting
{
    public class Zako4 : Enemy
    {
        const int speed = 5;
        const int size = 64;

        int count = 0;
        float angle;
        bool inPlayingField = false;
        float pastX;
        float pastY;
        float vx;
        float vy;
        //list to place mines
        List<Mine> mines = new List<Mine>();

        public Zako4(Game game, float x, float y, int HPMultiplier)
            : base(game, x, y)
        {
            life = 50 * HPMultiplier;
            angle = MyMath.PointToPointAngle(x, y, game.player.x, game.player.y);
            vx = (float)Math.Cos(angle) * speed;
            vy = (float)Math.Sin(angle) * speed;
        }

        public override void Update()
        {
            foreach (Mine mine in mines) mine.Update();
            MineCollisionWithPlayerBullet();
            MineCollisionWithPlayer();
            PointsForBlowingUpMine();
            mines.RemoveAll(m => m.isDead);

            pastX = x;
            pastY = y;
            x += vx;
            y += vy;
            angle = MyMath.PointToPointAngle(pastX, pastY, x, y);

            if (inPlayingField)
            { //boundaries
                if (x < 0 || x > Screen.Width) vx = -vx;
                if (y < 0 || y > Screen.Height) vy = -vy;
            }
            else if (x > 0 && x < Screen.Width && y > 0 && y < Screen.Height) inPlayingField = true; //once in the playing field, inforce boudaries           

            count++;
            if (count == 50)
            {
                count = 0;
                mines.Add(new Mine(game, x, y));
            }
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, angle, Image.zako4);
            foreach (Mine mine in mines) mine.Draw();
        }

        public override void OnCollisionPlayerBullet(PlayerBullet playerBullet)
        {//mine takes damage
            life -= 1;

            if (life == 0)
            {
                isDead = true;
                game.explosions.Add(new Explosion(x, y));
                foreach (Mine mine in mines) mine.OnCollisionPlayer(); //kills all mines when this dies
            }
        }

        void MineCollisionWithPlayerBullet()
        {//mine takes damage if hit with a player bullet
            foreach (PlayerBullet playerBullet in game.playerBullets)
            {
                if (playerBullet.isDead) continue;

                foreach (Mine mine in mines)
                {
                    if (mine.isDead)continue;                   

                    if (MyMath.SphereSphereIntersection(
                        playerBullet.x, playerBullet.y, playerBullet.collisionRadius,
                        mine.x, mine.y, mine.collisionRadius))
                    {
                        mine.OnCollisionPlayerBullet(playerBullet);
                        playerBullet.OnCollisionEnemy(mine);

                        if (playerBullet.isDead) break;
                    }
                }
            }
        }

        void MineCollisionWithPlayer()
        {//player takes damage if they run into a mine
            foreach (Mine mine in mines)
            {
                if (game.player.isDead) break;

                if (mine.isDead) continue;

                if (MyMath.SphereSphereIntersection(
                        game.player.x, game.player.y, game.player.collisionRadius,
                        mine.x, mine.y, mine.collisionRadius))
                {
                    game.player.OnCollisionEnemy(mine);
                    mine.OnCollisionPlayer();
                }
            }
        }

        void PointsForBlowingUpMine()
        {//1 point for blowing up a mine
            foreach (Mine mine in mines)
            {
                if (mine.isDead) game.score += 1;
            }
        }
    }
}
