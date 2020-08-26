using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting
{
    public abstract class Enemy
    {
        public float x;
        public float y;
        public float collisionRadius = 32;
        public bool isDead = false;
        public int killCount = 0; //kill timer

        protected Game game;
        protected float life = 1;

        public Enemy(Game game, float x, float y)
        {
            this.game = game;
            this.x = x;
            this.y = y;
        }

        public abstract void Update();

        public abstract void Draw();

        public virtual void OnCollisionPlayerBullet(PlayerBullet playerBullet)
        {
            life -= 1;

            if (life == 0)
            {
                isDead = true;
                game.explosions.Add(new Explosion(x, y));
            }

            if (playerBullet is MissileBullet)
            {//extra sounds and effects if hit by a missile
                Sound.Play(Sound.explosion);
                game.explosions.Add(new Explosion(playerBullet.x, playerBullet.y));
            }
        }

        public virtual void OnCollisionPlayer()
        {          
        }

        public virtual void OutOfBoundsKillFlag()
        {
            if (x < 0 || x > Screen.Width || y < 0 || y > Screen.Height)
            {
                killCount++;
                if (killCount >= 200)
                {
                    isDead = true;
                }
            }
            else killCount = 0;
        }
    }
}
