using DxLibDLL;

namespace Shooting
{
    public class Explosion
    {
        public float x;
        public float y;
        public bool isDead = false;

        int counter = 0;
        int imageIndex = 0;

        public Explosion(float x, float y)
        {
            this.x = x;
            this.y = y;
            Sound.Play(Sound.explosion);
        }

        public void Update()
        {
            counter++;

            imageIndex = counter / 3;

            if (imageIndex >= 16)
            {
                isDead = true;
            }
        }

        public void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1, 0, Image.explosion[imageIndex]);
        }
    }
}
