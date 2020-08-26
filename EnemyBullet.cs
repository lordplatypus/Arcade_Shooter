using System;
using DxLibDLL;

namespace Shooting
{
    public class EnemyBullet
    {
        const float VisibleRadius = 8f;

        public float x;
        public float y;
        public bool isDead = false;
        public float collisionRadius = 8f;

        float vx;
        float vy;
        int count = 0;
        int imageIndex = 0;

        public EnemyBullet(float x, float y, float angle, float speed)
        {
            this.x = x;
            this.y = y;
            vx = (float)Math.Cos(angle) * speed;
            vy = (float)Math.Sin(angle) * speed;
        }

        public void Update()
        {
            x += vx;
            y += vy;

            if (y + VisibleRadius < 0 || y - VisibleRadius > Screen.Height ||
                x + VisibleRadius < 0 || x - VisibleRadius > Screen.Width)
            {
                isDead = true;
            }
            count++;
            imageIndex = count / 10 % 8;
        }

        public void Draw()
        {
            if (vx <= 0) DX.DrawRotaGraphF(x, y, 1, 0f, Image.letter[imageIndex]);
            if (vx > 0) DX.DrawRotaGraphF(x, y, 1, 0f, Image.letter[imageIndex], DX.TRUE, DX.TRUE);
        }
    }
}
