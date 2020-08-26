using System;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    public class MissileBullet : PlayerBullet
    {
        protected Game game;
        const float speed = .2f;
        const float maxSpeed = 3f;
        const float brake = .4f;

        int count = 0;
        int imageIndex;

        float vx; 
        float vy;
        float angleToEnemy;
        float angleX;
        float angleXPast = 0;
        float angleYPast = 0;
        float angleY;

        public MissileBullet(float x, float y, float angle, Game game) : base(x, y, angle)
        {
            this.game = game;
        }

        public override void Update()
        {
            count++;

            if (count == 200) isDead = true; //after 200 frames missile detonates

            if (x + VisibleRadius < 0 || x - VisibleRadius > Screen.Width ||
                y + VisibleRadius < 0 || y - VisibleRadius > Screen.Width)
            {
                isDead = true;
            }

            imageIndex = count / 10 % 2; //for missile animation

            if (count < 50)
            {//for the first 50 frames, move toward mouse
                x += (float)Math.Cos(angle) * 3f;
                y += (float)Math.Sin(angle) * 3f;
                return;
            }

            angleX = (float)Math.Cos(angleToEnemy);
            angleY = (float)Math.Sin(angleToEnemy);

            if (angleX != angleXPast) x -= brake;
            if (angleY != angleYPast) y -= brake;

            angleXPast = angleX;
            angleYPast = angleY;

            vx += angleX * speed;
            vy += angleY * speed;
            if (vx > maxSpeed) vx = maxSpeed;
            if (vy > maxSpeed) vy = maxSpeed;

            x += vx;
            y += vy;
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, angleToEnemy, Image.missile[imageIndex]);
            if (count == 199) game.explosions.Add(new Explosion(x, y));
        }

        public void AngleToEnemy(float x, float y)
        {
            angleToEnemy = MyMath.PointToPointAngle(this.x, this.y, x, y);
        }
    }
}
