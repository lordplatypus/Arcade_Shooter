using System;
using System.Collections.Generic;
using DxLibDLL;
using MyLib;

namespace Shooting
{
    public class Wave
    {
        enum State
        {
            Normal,
            Boss,
        }

        Game game;
        State state;

        public int waveCount;
        int enemySpawnCount = 0;
        int maxEnemyCount = 10;
        int screenSpawnPosition;

        public int HPMultiplier;

        public Wave(Game game)
        {
            this.game = game;
            state = State.Normal;
            waveCount = 1;
            HPMultiplier = 1;
        }

        public void Update()
        {
            if (state == State.Normal)
            {
                if (enemySpawnCount < maxEnemyCount)
                {
                    if (game.enemies.Count < waveCount * 2)
                    {
                        enemySpawnCount++;
                        screenSpawnPosition = MyRandom.Range(0, 4); //spawn enemy from one side
                        if (screenSpawnPosition == 0)
                        {
                            game.enemies.Add(new Zako3(game, Screen.Width, MyRandom.Range(0, Screen.Height), HPMultiplier));
                            if (MyRandom.Range(0,20) == 0) game.enemies.Add(new Zako4(game, Screen.Width, MyRandom.Range(0, Screen.Height), HPMultiplier));
                        }
                        if (screenSpawnPosition == 1)
                        {
                            game.enemies.Add(new Zako3(game, -64, MyRandom.Range(0, Screen.Height), HPMultiplier));
                            if (MyRandom.Range(0, 20) == 0) game.enemies.Add(new Zako4(game, -64, MyRandom.Range(0, Screen.Height), HPMultiplier));
                        }
                        if (screenSpawnPosition == 2)
                        {
                            game.enemies.Add(new Zako3(game, MyRandom.Range(0, Screen.Width), Screen.Height, HPMultiplier));
                            if (MyRandom.Range(0, 20) == 0) game.enemies.Add(new Zako4(game, MyRandom.Range(0, Screen.Width), Screen.Height, HPMultiplier));
                        }
                        else
                        {
                            game.enemies.Add(new Zako3(game, MyRandom.Range(0, Screen.Width), -64, HPMultiplier));
                            if (MyRandom.Range(0, 20) == 0) game.enemies.Add(new Zako4(game, MyRandom.Range(0, Screen.Width), -64, HPMultiplier));
                        }
                    }
                }
                if (enemySpawnCount >= maxEnemyCount && game.enemies.Count == 0)
                {
                    for (int i = 0; i < waveCount; i++)
                    {
                        screenSpawnPosition = MyRandom.Range(0, 4);
                        if (screenSpawnPosition == 0) game.enemies.Add(new Boss2(game, Screen.Width, MyRandom.Range(0, Screen.Height), this, HPMultiplier));
                        else if (screenSpawnPosition == 1) game.enemies.Add(new Boss2(game, 0, MyRandom.Range(0, Screen.Height), this, HPMultiplier));
                        else if (screenSpawnPosition == 2) game.enemies.Add(new Boss2(game, MyRandom.Range(0, Screen.Width), Screen.Height, this, HPMultiplier));
                        else game.enemies.Add(new Boss2(game, MyRandom.Range(0, Screen.Width), 0, this, HPMultiplier));
                    }                
                    state = State.Boss;
                }
            }
            if (state == State.Boss)
            {
                if (game.enemies.Count == 0)
                {
                    if (game.gameType == Game.GameType.Endless)
                    {
                        waveCount++;
                        maxEnemyCount *= waveCount;
                    }
                    if (game.gameType == Game.GameType.EndlessHP)
                    {
                        HPMultiplier++;
                    }
                    enemySpawnCount = 0;
                    state = State.Normal;
                }
            }
        }
    }
}
