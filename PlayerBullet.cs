using System;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    public abstract class PlayerBullet
    {
        public const float Speed = 25f;
        public const int VisibleRadius = 16;

        public float x;
        public float y;
        public bool isDead = false;
        public float collisionRadius = 16f;

        public float angle;
        public float count;
        public int powerUpCount;

        public PlayerBullet(float x, float y, float angle)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
            Sound.Play(Sound.laser);
        }

        public abstract void Update();

        public abstract void Draw();

        public virtual void OnCollisionEnemy(Enemy enemy)
        {
            isDead = true;
        }       
    }
}
