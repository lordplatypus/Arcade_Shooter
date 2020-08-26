using DxLibDLL;

namespace Shooting
{
    public static class Image
    {
        //Player / Player Bullet Images
        public static int player;
        public static int gunStright;
        public static int gunSpread;
        public static int gunMissile;
        public static int[] missile = new int[2];
        public static int playerBullet;
        public static int strightBullet;
        public static int spreadBullet;

        //Item Images
        public static int healthItem;
        public static int strightItem;
        public static int spreadItem;
        public static int missileItem;

        //Enemy / Enemy Bullet Images
        public static int zako0;
        public static int zako1;
        public static int zako2;
        public static int zako3;
        public static int zako4;
        public static int enemyBullet16;
        public static int boss1;
        public static int boss2;
        public static int boss3;
        public static int teleportingSight;
        public static int turrent;
        public static int mine;
        public static int[] letter = new int[8];

        //Misc. Images
        public static int BG;
        public static int sight;
        public static int[] explosion = new int[16];

        public static void Load()
        {
            //Player / Player Bullet Images
            player = DX.LoadGraph("Image/Player/tank.png");
            //changes the gun on top of the tank depending on weapon type
            gunStright = DX.LoadGraph("Image/Player/player_stright.png");
            gunSpread = DX.LoadGraph("Image/Player/player_spread.png");
            gunMissile = DX.LoadGraph("Image/Player/player_missile.png");
            //missile animation
            DX.LoadDivGraph("Image/Player/missile_animation.png", 2, 2, 1, 32, 32, missile);
            //regular player bullets
            playerBullet = DX.LoadGraph("Image/Player/projectile-orange.png");
            strightBullet = DX.LoadGraph("Image/Player/projectile-red.png");
            spreadBullet = DX.LoadGraph("Image/Player/projectile-blue.png");

            //Item Images
            healthItem = DX.LoadGraph("Image/Item/heart_item.png");
            strightItem = DX.LoadGraph("Image/Item/stright_item.png");
            spreadItem = DX.LoadGraph("Image/Item/spread_item.png");
            missileItem = DX.LoadGraph("Image/Item/missile_item.png");

            //Enemy / Enemy Bullet Images
            //basic enemies
            zako0 = DX.LoadGraph("Image/Enemy/zako0.png");
            zako1 = DX.LoadGraph("Image/Enemy/zako1.png");
            zako2 = DX.LoadGraph("Image/Enemy/zako2.png");
            zako3 = DX.LoadGraph("Image/Enemy/zako3.png");
            zako4 = DX.LoadGraph("Image/Enemy/zako4.png");
            //boss phases
            boss1 = DX.LoadGraph("Image/Enemy/Boss/large_blue_01.png");
            boss2 = DX.LoadGraph("Image/Enemy/Boss/boss2.png");
            boss3 = DX.LoadGraph("Image/Enemy/Boss/large_blue_02.png");
            teleportingSight = DX.LoadGraph("Image/Enemy/Boss/teleporting_sight.png");
            turrent = DX.LoadGraph("Image/Enemy/Boss/turrent.png");
            //mine
            mine = DX.LoadGraph("Image/Enemy/bomb.png");
            //Enemy 'bullet' animation
            DX.LoadDivGraph("Image/Enemy/Flying_Love_Letter.png", 8, 8, 1, 32, 32, letter);

            //Misc. Images
            //background image
            BG = DX.LoadGraph("Image/Other/map.png");
            //mouse pointer
            sight = DX.LoadGraph("Image/Other/Sight.png");
            //explosion animation
            DX.LoadDivGraph("Image/Other/explosion.png", 16, 8, 2, 64, 64, explosion);
        }
    }
}
