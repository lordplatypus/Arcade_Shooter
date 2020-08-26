using System.Collections.Generic;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    public class Game
    {
        enum State
        {//main game states
            Title,
            Help,
            Options,
            Restart,
            Game,
            Pause,
            Clear,
            End,
        }
        public enum GameType
        {//game types
            Normal,
            Endless,
            EndlessHP,
        }

        enum Option
        {//options
            Neutral,
            Music,
            BGMVolume,
            SEVolume,
        }

        public Player player;
        public Mouse mouse;
        public List<PlayerBullet> playerBullets;
        public List<Enemy> enemies;
        public List<EnemyBullet> enemyBullets;
        public List<Explosion> explosions;
        public List<Item> items;
        public float scrollSpeed = 1.5f;
        public Map map;
        Wave wave;

        //Game states
        State state;
        //Game type - normal or endless
        public GameType gameType;
        //Option selection (currently only one)
        Option option;

        //Used to calculate the closest enemy relative to a fired missile
        float closeEnemyX;
        float closeEnemyY;

        //Score system
        public int score;
        int maxScore0; //max score normal mode
        int maxScore1; //max score endless mode
        int maxScore2; //max score endless hp mode

        //For the title menu
        int menuCount = 0;
        int startCountDown = 0;
        bool optionSelected = false;
        string[] titleText = new string[5];

        //For options
        int optionCount = 0;
        int musicCount = 0;

        //Bool to check if the game was just paused
        //needed because the game will just pause again after un-pausing
        bool pause = false;

        public void Init()
        {
            //initial setup
            Input.Init();
            MyRandom.Init();
            Image.Load();
            Sound.Load();
            DX.SetMouseDispFlag(DX.FALSE);

            //first state as the game starts is 'title'
            state = State.Title;

            //sets menu text
            DX.ChangeFont("Blue Sky 8x8");
            titleText[0] = "NORMAL MODE";
            titleText[1] = "ENDLESS MODE";
            titleText[2] = "ENDLESS MODE - HP";
            titleText[3] = "HELP";
            titleText[4] = "OPTIONS";

            //sets max score to 0
            maxScore0 = 0;
            maxScore1 = 0;
            maxScore2 = 0;
        }

        void Restart()
        {
            //resets lists
            wave = new Wave(this);
            mouse = new Mouse();
            player = new Player(Screen.Width / 2, Screen.Height / 2, this, mouse);
            playerBullets = new List<PlayerBullet>();
            enemies = new List<Enemy>();
            enemyBullets = new List<EnemyBullet>();
            explosions = new List<Explosion>();
            items = new List<Item>();

            //sets state to 'Game'
            state = State.Game;

            //resets map
            map = new Map(this, wave, 0, "Map/stage1.csv");

            //resets score
            score = 0;
        }

        public void Update()
        {
            Input.Update(); //updates inputs

            if (state == State.Title)
            {
                Sound.BGM(Sound.BGMMenu); //play menu music when returning to title screen

                if (Input.GetButtonDown(DX.PAD_INPUT_8) || Input.GetButtonDown(DX.PAD_INPUT_UP))
                {//move up
                    Sound.Play(Sound.menuMovement);
                    menuCount--;
                    if (menuCount < 0) menuCount = 4; //loop
                }
                else if (Input.GetButtonDown(DX.PAD_INPUT_5) || Input.GetButtonDown(DX.PAD_INPUT_DOWN))
                {//move down
                    Sound.Play(Sound.menuMovement);
                    menuCount++;
                    if (menuCount > 4) menuCount = 0;
                }

                //Pressing Z triggers the bool which in turn activates the 'countdown'
                //once the 'countdown' counts 60 frames, activate selected option
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    Sound.Play(Sound.menuSelect);
                    optionSelected = true;
                }
                if (optionSelected) startCountDown++;
                if (menuCount == 0 && startCountDown == 60)
                {//Normal mode
                    state = State.Restart;
                    optionSelected = false; //reset bool
                    startCountDown = 0; //reset 'countdown'
                    gameType = GameType.Normal;
                }
                if (menuCount == 1 && startCountDown == 60)
                {//Endless mode
                    state = State.Restart;
                    optionSelected = false;
                    startCountDown = 0;
                    gameType = GameType.Endless;
                }
                if (menuCount == 2 && startCountDown == 60)
                {//Endless mode - HP
                    state = State.Restart;
                    optionSelected = false;
                    startCountDown = 0;
                    gameType = GameType.EndlessHP;
                }
                if (menuCount == 3 && startCountDown == 60)
                {//Help
                    optionSelected = false;
                    startCountDown = 0;
                    state = State.Help;
                }
                if (menuCount == 4 && startCountDown == 60)
                {//Options
                    optionSelected = false;
                    startCountDown = 0;
                    state = State.Options;
                }
            }

            if (state == State.Options)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_2) && option == Option.Neutral)
                {//go back to menu
                    Sound.Play(Sound.cancel);
                    state = State.Title;
                }

                //Cicle through the options
                if (option == Option.Neutral)
                {
                    if (Input.GetButtonDown(DX.PAD_INPUT_8) || Input.GetButtonDown(DX.PAD_INPUT_UP))
                    {//Up
                        Sound.Play(Sound.menuMovement);
                        optionCount--;
                        if (optionCount < 0) optionCount = 2;
                    }
                    else if (Input.GetButtonDown(DX.PAD_INPUT_5) || Input.GetButtonDown(DX.PAD_INPUT_DOWN))
                    {//down
                        Sound.Play(Sound.menuMovement);
                        optionCount++;
                        if (optionCount > 2) optionCount = 0;
                    }
                }

                //If music option was selected
                if (Input.GetButtonDown(DX.PAD_INPUT_1) && optionCount == 0) option = Option.Music;
                if (option == Option.Music)
                {//now the player can cicle through the song list
                    if (Input.GetButtonDown(DX.PAD_INPUT_2))
                    {//back out of music selection
                        Sound.Play(Sound.cancel);
                        option = Option.Neutral;
                    }
                    if (Input.GetButtonDown(DX.PAD_INPUT_8) || Input.GetButtonDown(DX.PAD_INPUT_UP))
                    {//scroll list up
                        Sound.Play(Sound.menuMovement);
                        musicCount--;
                        if (musicCount < 0) musicCount = 3;
                    }
                    else if (Input.GetButtonDown(DX.PAD_INPUT_5) || Input.GetButtonDown(DX.PAD_INPUT_DOWN))
                    {//scroll list down
                        Sound.Play(Sound.menuMovement);
                        musicCount++;
                        if (musicCount > 3) musicCount = 0;
                    }
                    if (Input.GetButtonDown(DX.PAD_INPUT_1))
                    {//select music that will be played during the game
                        Sound.Play(Sound.menuSelect);
                        if (musicCount == 0) Sound.musicID = 0;
                        if (musicCount == 1) Sound.musicID = 1;
                        if (musicCount == 2) Sound.musicID = 2;
                        if (musicCount == 3) Sound.musicID = 3;
                        Sound.SetMusic();
                    }
                }

                //if BGM volume option was selected
                if (Input.GetButtonDown(DX.PAD_INPUT_1) && optionCount == 1)
                {
                    option = Option.BGMVolume;
                    Sound.Play(Sound.menuSelect);
                }
                if (option == Option.BGMVolume)
                {
                    if (Input.GetButtonDown(DX.PAD_INPUT_2))
                    {//back out of BGM Volume setting
                        Sound.Play(Sound.cancel);
                        option = Option.Neutral;
                    }
                    if (Input.GetButtonDown(DX.PAD_INPUT_4) || Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                    {//press left to lower the volume by 10
                        Sound.Play(Sound.menuMovement);
                        Sound.BGMVolume -= 10;
                        if (Sound.BGMVolume < 0) Sound.BGMVolume = 0;
                        Sound.SetVolume(); //updates the sounds so they actually change volume
                    }
                    if (Input.GetButtonDown(DX.PAD_INPUT_6) || Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                    {//press right to increase the volume by 10
                        Sound.Play(Sound.menuMovement);
                        Sound.BGMVolume += 10;
                        if (Sound.BGMVolume > 200) Sound.BGMVolume = 200;
                        Sound.SetVolume();
                    }
                }

                //if SE volume option was selected
                if (Input.GetButtonDown(DX.PAD_INPUT_1) && optionCount == 2)
                {
                    option = Option.SEVolume;
                    Sound.Play(Sound.menuSelect);
                }
                if (option == Option.SEVolume)
                {
                    if (Input.GetButtonDown(DX.PAD_INPUT_2))
                    {//back out of music selection
                        Sound.Play(Sound.cancel);
                        option = Option.Neutral;
                    }
                    if (Input.GetButtonDown(DX.PAD_INPUT_4) || Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                    {//press left to lower the volume by 10
                        Sound.Play(Sound.menuMovement);
                        Sound.SEVolume -= 10;
                        if (Sound.SEVolume < 0) Sound.SEVolume = 0;
                        Sound.SetVolume();
                    }
                    if (Input.GetButtonDown(DX.PAD_INPUT_6) || Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                    {//press right to increase the volume by 10
                        Sound.Play(Sound.menuMovement);
                        Sound.SEVolume += 10;
                        if (Sound.SEVolume > 200) Sound.SEVolume = 200;
                        Sound.SetVolume();
                    }
                }
            }

            if (state == State.Help)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_2))
                {
                    Sound.Play(Sound.cancel);
                    state = State.Title;
                }
            }

            if (state == State.Restart) Restart();

            if (state == State.Pause)
            {//Paused state, press Q again to un-pause
                if (Input.GetButtonDown(DX.PAD_INPUT_7)) state = State.Game;
                pause = true;
            }

            if (state == State.Game || state == State.End || state == State.Clear)
            {
                Sound.SetMusic(); //plays set music

                if (gameType == GameType.Normal)
                {
                    map.Scroll(scrollSpeed);
                    if (map.enemyCount == 0 && enemies.Count == 0) state = State.Clear;
                }

                if (gameType == GameType.Endless || gameType == GameType.EndlessHP) wave.Update();

                if (!player.isDead)
                {
                    mouse.Update();
                    player.Update();
                }
                else state = State.End;

                foreach (PlayerBullet b in playerBullets)
                {
                    b.Update();
                }

                foreach (Enemy e in enemies)
                {
                    e.Update();
                }

                foreach (EnemyBullet b in enemyBullets)
                {
                    b.Update();
                }

                foreach (Item i in items)
                {
                    i.Update();
                }

                foreach (EnemyBullet enemyBullet in enemyBullets)
                {//player takes damage from enemy bullets
                    if (player.isDead) break;

                    if (enemyBullet.isDead) continue;

                    if (MyMath.SphereSphereIntersection(
                        player.x, player.y, player.collisionRadius,
                        enemyBullet.x, enemyBullet.y, enemyBullet.collisionRadius))
                    {
                        player.OnCollisionEnemyBullet(enemyBullet);
                    }
                }

                foreach (PlayerBullet playerBullet in playerBullets)
                {
                    if (playerBullet.isDead) continue;

                    //for missiles
                    if (playerBullet is MissileBullet)
                    {
                        MissileBullet missile = (MissileBullet)playerBullet;
                        float shortestDistance = float.MaxValue;
                        foreach (Enemy enemy in enemies)
                        {//missile looks for closest enemy
                            if (MyMath.DistanceBetweenTwoPoints(missile.x, missile.y, enemy.x, enemy.y) < shortestDistance)
                            {
                                shortestDistance = MyMath.DistanceBetweenTwoPoints(missile.x, missile.y, enemy.x, enemy.y);
                                closeEnemyX = enemy.x;
                                closeEnemyY = enemy.y;
                            }
                            missile.AngleToEnemy(closeEnemyX, closeEnemyY);
                        }
                    }

                    //enemies take damage if hit by player bullet
                    foreach (Enemy enemy in enemies)
                    {
                        if (enemy.isDead) continue;

                        if (enemy is Boss2)
                        {//if boss2 is in normal state (or when it is teleporting) it can't be hit by bullets - this way only the turrents can be hit
                            Boss2 boss = (Boss2)enemy;
                            if (boss.state == Boss2.State.Normal || boss.state == Boss2.State.Invincible) continue;
                        }

                        if (MyMath.SphereSphereIntersection(
                            playerBullet.x, playerBullet.y, playerBullet.collisionRadius,
                            enemy.x, enemy.y, enemy.collisionRadius))
                        {
                            enemy.OnCollisionPlayerBullet(playerBullet);
                            playerBullet.OnCollisionEnemy(enemy);
                            
                            if (playerBullet.isDead) break;
                        }
                    }
                }

                foreach (Enemy enemy in enemies)
                {
                    if (player.isDead) break;

                    if (enemy.isDead) continue;

                    if (enemy is Boss2)
                    {//can't hurt player when boss is teleporting
                        Boss2 boss = (Boss2)enemy;
                        if (boss.state == Boss2.State.Invincible) continue;
                    }

                    if (MyMath.SphereSphereIntersection(
                            player.x, player.y, player.collisionRadius,
                            enemy.x, enemy.y, enemy.collisionRadius))
                    {
                        player.OnCollisionEnemy(enemy);
                        enemy.OnCollisionPlayer();
                    }

                    enemy.OutOfBoundsKillFlag(); //enemies are removed if they are outside the boundaries for too long
                }

                //this decides item drop chance and score
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.isDead)
                    {
                        //items have a 1 in 10 chance to drop something
                        int itemDrop = MyRandom.Range(0, 10);
                        if (itemDrop == 0) items.Add(new HealthItem(enemy));
                        if (itemDrop == 1) items.Add(new StrightBulletPowerUpItem(enemy));
                        if (itemDrop == 2) items.Add(new SpreadBulletPowerUpItem(enemy));
                        if (itemDrop == 3) items.Add(new MissileBulletPowerUpItem(enemy));

                        //most enemies are worth 10 points, 1 type of enemy is worth 20, and the boss is worth 100
                        //Note - points for mines and boss turrents are calculated in Zako4 and Boss2 respectivly
                        if (enemy is Zako0 || enemy is Zako1 || enemy is Zako2 || enemy is Zako3) score += 10;
                        if (enemy is Zako4) score += 20;
                        if (enemy is Boss2) score += 100;
                    }
                }

                //updates items and detects if the player picks them up
                foreach (Item item in items)
                {
                    if (player.isDead) break;

                    if (item.isDead) continue;

                    if (MyMath.SphereSphereIntersection(
                            player.x, player.y, player.collisionRadius,
                            item.x, item.y, item.collisionRadius))
                    {
                        player.OnCollisionItem(item);
                        item.OnCollisionPlayer(player);
                    }
                }

                foreach (Explosion e in explosions)
                {
                    e.Update();
                }

                //removes all assets that are no longer used
                playerBullets.RemoveAll(pb => pb.isDead);
                enemies.RemoveAll(e => e.isDead);
                enemyBullets.RemoveAll(eb => eb.isDead);
                explosions.RemoveAll(e => e.isDead);
                items.RemoveAll(i => i.isDead);

                if (state == State.End || state == State.Clear)
                {//only save score if the player finishes or dies on a stage
                    //if current score is higher then max score, change max score to the current score
                    if (gameType == GameType.Normal && score > maxScore0) maxScore0 = score;
                    if (gameType == GameType.Endless && score > maxScore1) maxScore1 = score;
                    if (gameType == GameType.EndlessHP && score > maxScore2) maxScore2 = score;
                }

                //player can reset or go back to the title screen at any time
                if (Input.GetButtonDown(DX.PAD_INPUT_1)) state = State.Restart;
                if (Input.GetButtonDown(DX.PAD_INPUT_2)) state = State.Title;

                //Press Q to pause
                if (Input.GetButtonDown(DX.PAD_INPUT_7) && pause == false) state = State.Pause;
                if (pause == true) pause = false;
            }
        }

        public void Draw()
        {
            if (state == State.Title)
            {
                string[] mainMenu = new string[]
                {
                    "TITLE SCREEN",
                    "Use UP and DOWN keys to go through the options, Press the Z key to select an option"
                };
                for (int i = 0; i < mainMenu.Length; i++)
                {
                    DX.DrawString(0, i * 20, mainMenu[i], DX.GetColor(255, 255, 255));
                }

                if (startCountDown / 5 % 2 == 0) DX.DrawString(0, 60, titleText[menuCount], DX.GetColor(0, 255, 255));

                //display max scores on the title screen
                DX.DrawString(0, 100, "Normal Mode - High Score: " + maxScore0, DX.GetColor(255, 255, 255));
                DX.DrawString(0, 120, "Endless Mode - High Score: " + maxScore1, DX.GetColor(255, 255, 255));
                DX.DrawString(0, 140, "Endless Mode - HP - High Score: " + maxScore2, DX.GetColor(255, 255, 255));
            }

            if (state == State.Options)
            {
                //selected item changes color
                uint[] optionSelectColor = new uint[3];
                for (int i = 0; i < optionSelectColor.Length; i++)
                {
                    if (i == optionCount) optionSelectColor[i] = DX.GetColor(0, 255, 255);
                    if (i != optionCount) optionSelectColor[i] = DX.GetColor(255, 255, 255);
                }
                DX.DrawString(0, 0, "Use the Up and Down keys to highlight an option and use Z to select it", DX.GetColor(255, 255, 255));
                DX.DrawString(0, 20, "Press X to go back to the title", DX.GetColor(255, 255, 255));

                DX.DrawString(0, 60, "Select Music: ", optionSelectColor[0]);
                DX.DrawString(0, 100, "Music Volume: " + Sound.BGMVolume, optionSelectColor[1]);
                DX.DrawString(0, 140, "Sound Effect Volume: " + Sound.SEVolume, optionSelectColor[2]);

                //music titles
                string[] musicTitles = new string[]
                {
                    "Isometric",
                    "Scoop film Reel",
                    "Necro funk the around",
                    "Dream vanishing point"
                };
                //color selection for music titles
                uint[] musicSelectColor = new uint[4];
                for (int i = 0; i< musicSelectColor.Length; i++)
                {
                    if (i == Sound.musicID) musicSelectColor[i] = DX.GetColor(0, 255, 255);
                    else musicSelectColor[i] = DX.GetColor(255, 255, 255);
                }
                DX.DrawString(300, 60, musicTitles[musicCount], musicSelectColor[musicCount]);   

            }

            if (state == State.Help)
            {
                string[] helpMenu = new string[]
                {
                    "Press X to go back to the title",
                    "+Use the 'wasd' keys to control the tank",
                    "+Use the mouse to aim the gun",
                    "+Press either the left or right mouse button ",
                    "to fire the gun",
                    "+Press Q to pause/un-pause",
                    "+Press the Z key during the game to quick reset",
                    "+Press the X key during the game to return to the title",
                    "+NORMAL MODE - Enemies appear on the left based on a timer",
                    "+ENDLESS MODE - Enemies come in waves, ",
                    "later waves will have more enemies",
                    "+ENDLESS MODE - HP - Enemies come in waves, every time a",
                    "new wave starts all enemies will have their health ",
                    "increased by 2 times", 
                    "+Endless mode won't end until the player dies"
                };

                for (int i = 0; i < helpMenu.Length; i++)
                {
                    DX.DrawString(0, i * 20, helpMenu[i], DX.GetColor(255, 255, 255));
                }
            }

            if (state == State.Game || state == State.Pause || state == State.Clear || state == State.End)
            {
                DX.DrawGraph(0, 0, Image.BG); //background image
                DX.DrawBox(0, 0, Screen.Width, 16, DX.GetColor(0, 0, 0), DX.TRUE); //black box to make text easy to read

                foreach (Enemy e in enemies)
                {
                    e.Draw();
                }

                foreach (Explosion e in explosions)
                {
                    e.Draw();
                }

                if (!player.isDead)
                {
                    mouse.Draw();
                    player.Draw();
                }

                foreach (PlayerBullet b in playerBullets)
                {
                    b.Draw();
                }

                foreach (EnemyBullet b in enemyBullets)
                {
                    b.Draw();
                }

                foreach (Item i in items)
                {
                    i.Draw();
                }

                //Current Player health and score
                DX.DrawString(0, 0, "Health: " + player.life, DX.GetColor(255, 255, 255));
                DX.DrawString(200, 0, "Score: " + score, DX.GetColor(255, 255, 255));

                //Display high score and wave number
                if (gameType == GameType.Normal) DX.DrawString(400, 0, "High Score: " + maxScore0, DX.GetColor(255, 255, 255));
                if (gameType == GameType.Endless)
                {
                    DX.DrawString(400, 0, "High Score: " + maxScore1, DX.GetColor(255, 255, 255));
                    DX.DrawString(700, 0, "Wave: " + wave.waveCount, DX.GetColor(255, 255, 255));
                }
                if (gameType == GameType.EndlessHP)
                {
                    DX.DrawString(400, 0, "High Score: " + maxScore2, DX.GetColor(255, 255, 255));
                    DX.DrawString(700, 0, "Wave: " + wave.HPMultiplier, DX.GetColor(255, 255, 255));
                }

                if (state == State.Pause)
                {//display 'Pause' when the game is paused
                    string pauseString = "PAUSE";
                    int stringLength = DX.GetDrawStringWidth(pauseString, pauseString.Length);
                    DX.DrawBox(Screen.Width / 2 - stringLength / 2, Screen.Height / 2 - 16 / 2, Screen.Width / 2 + stringLength / 2, Screen.Height / 2 + 16 / 2, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawString(Screen.Width / 2 - stringLength / 2, Screen.Height / 2 - 16 / 2, pauseString, DX.GetColor(255, 255, 255));
                }

                if (state == State.Clear)
                {//on clearing normal mode
                    DX.DrawString(0, 20, "Good job, you cleared the first stage", DX.GetColor(255, 255, 255));
                    DX.DrawString(0, 40, "(press Z to try again, or X to go back to title)", DX.GetColor(255, 255, 255));
                }

                if (state == State.End)
                {//on dying
                    DX.DrawString(0, 20, "Whoops, restart? (press Z to try again,", DX.GetColor(255, 255, 255));
                    DX.DrawString(0, 40, "or X to go back to title)", DX.GetColor(255, 255, 255));
                }
            }            
        }
    }
}
