using System.Diagnostics;
using System.IO;

namespace Shooting
{
    public class Map
    {
        const int Width = 1000;
        const int Height = 17;
        const int CellSize = 32;

        Game game;
        Wave wave;
        int[,] enemyData;
        float position;
        public int enemyCount;

        public Map(Game game, Wave wave, float startPosition, string filePath)
        {
            this.game = game;
            this.wave = wave;
            position = startPosition;
            Load(filePath);
        }

        void Load(string filePath)
        {
            enemyData = new int[Width, Height];

            string[] lines = File.ReadAllLines(filePath);

            Debug.Assert(lines.Length == Height, "" + lines.Length);

            for (int y = 0; y < Height; y++)
            {
                string[] splitted = lines[y].Split(new char[] { ',' });

                Debug.Assert(splitted.Length == Width, "" + y + "" + splitted.Length);

                for (int x = 0; x < Width; x++)
                {
                    enemyData[x, y] = int.Parse(splitted[x]);
                    if (enemyData[x, y] != -1 && enemyData[x, y] != 99) enemyCount++;
                }
            }
        }

        public void Scroll(float delta)
        {
            int prevRightCell = (int)(position + Screen.Width) / CellSize;

            position += delta;

            int currentRightCell = (int)(position + Screen.Width) / CellSize;

            if (currentRightCell >= Width) return;

            if (prevRightCell == currentRightCell) return;

            float x = currentRightCell * CellSize - position;

            for (int cellY = 0; cellY < Height; cellY++)
            {
                float y = cellY * CellSize;

                int id = enemyData[currentRightCell, cellY];

                if (id == -1) continue;
                else if (id == 0) game.enemies.Add(new Zako0(game, x + 32, y + 32));
                else if (id == 1) game.enemies.Add(new Zako1(game, x + 32, y + 32, 0));
                else if (id == 2) game.enemies.Add(new Zako2(game, x + 32, y + 32, 0));
                else if (id == 3) game.enemies.Add(new Zako3(game, x + 32, y + 32, 1));
                else if (id == 4) game.enemies.Add(new Zako4(game, x + 32, y + 32, 1));
                else if (id == 100) game.enemies.Add(new Boss2(game, x + 90, y + 32, wave, 1));
                else Debug.Assert(false, "" + id + "");
                enemyCount--;
            }
        }
    }
}
