using DxLibDLL;
using System;
using MyLib;

namespace Shooting
{
    public abstract class Item
    {
        public float x;
        public float y;
        public bool isDead = false;
        public int deathCounter;
        public float collisionRadius = 32;
        public float angleSpeed;

        public Item(Enemy enemy)
        {
            x = enemy.x;
            y = enemy.y;
            deathCounter = 0;
        }

        public virtual void Update()
        {
            deathCounter++;

            angleSpeed = deathCounter * MyMath.Deg2Rad;
            x += (float)Math.Cos(angleSpeed);
            y += (float)Math.Sin(angleSpeed);

            if (deathCounter > 500) isDead = true;
        }

        public abstract void Draw();       

        public virtual void OnCollisionPlayer(Player player)
        {
            Sound.Play(Sound.powerup);
            isDead = true;
        }
    }
}
