using DxLibDLL;

namespace Shooting
{
    public static class Sound
    {
        //Player
        public static int laser;
        public static int powerup;
        public static int oof;

        //UI
        public static int menuMovement;
        public static int menuSelect;
        public static int cancel;

        //BGM
        public static int BGM1;
        public static int BGM2;
        public static int BGM3;
        public static int BGM4;
        public static int BGMMenu;

        //Other
        public static int explosion;

        static bool play = false;
        static int oldHandle;
        public static int musicID = 0;
        public static int BGMVolume = 200;
        public static int SEVolume = 200;

        public static void Load()
        {
            //Player
            laser = DX.LoadSoundMem("SE/laser_se.wav");
            powerup = DX.LoadSoundMem("SE/powerup_se.wav");
            oof = DX.LoadSoundMem("SE/oof.mp3");

            //UI
            menuMovement = DX.LoadSoundMem("SE/UI01.wav");
            menuSelect = DX.LoadSoundMem("SE/UI02.wav");
            cancel = DX.LoadSoundMem("SE/UI03.wav");

            //BGM
            BGMMenu = DX.LoadSoundMem("BGM/lobby.mp3");
            BGM1 = DX.LoadSoundMem("BGM/Isometric.mp3");
            BGM2 = DX.LoadSoundMem("BGM/Scoop film Reel.wav");
            BGM3 = DX.LoadSoundMem("BGM/Necro funk the around.wav");
            BGM4 = DX.LoadSoundMem("BGM/Dream vanishing point.wav");

            //Other
            explosion = DX.LoadSoundMem("SE/explosion_se.wav");
        }

        public static void Play(int handle)
        {
            DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_BACK);
        }

        public static void BGM(int handle)
        {
            if (oldHandle != handle)
            {
                DX.StopSoundMem(oldHandle);
                play = false;
            }
            if (play == false)
            {
                DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_LOOP);
                oldHandle = handle;
                play = true;
            }           
        }

        public static void SetMusic()
        {
            if (musicID == 0) BGM(BGM1);
            if (musicID == 1) BGM(BGM2);
            if (musicID == 2) BGM(BGM3);
            if (musicID == 3) BGM(BGM4);
        }

        public static void SetVolume()
        {
            DX.ChangeVolumeSoundMem(SEVolume, laser);
            DX.ChangeVolumeSoundMem(SEVolume, powerup);
            DX.ChangeVolumeSoundMem(SEVolume, oof);
            DX.ChangeVolumeSoundMem(SEVolume, menuMovement);
            DX.ChangeVolumeSoundMem(SEVolume, menuSelect);
            DX.ChangeVolumeSoundMem(SEVolume, cancel);
            DX.ChangeVolumeSoundMem(BGMVolume, BGMMenu);
            DX.ChangeVolumeSoundMem(BGMVolume, BGM1);
            DX.ChangeVolumeSoundMem(BGMVolume, BGM2);
            DX.ChangeVolumeSoundMem(BGMVolume, BGM2);
            DX.ChangeVolumeSoundMem(BGMVolume, BGM3);
            DX.ChangeVolumeSoundMem(SEVolume, explosion); 
        }
    }
}
