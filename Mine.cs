using System;
using DxLibDLL;
using MyLib;


namespace Shooting
{
    public class Mine : Enemy
    {
        public Mine(Game game, float x, float y)
            : base(game, x, y)
        {
            life = 5;
        }

        public override void Update()
        {
            
        }

        public override void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, 0, Image.mine);
        }

        public override void OnCollisionPlayer()
        {
            life -= 5;

            if (life <= 0)
            {
                isDead = true;
                game.explosions.Add(new Explosion(x, y));
            }
        }
    }
}
