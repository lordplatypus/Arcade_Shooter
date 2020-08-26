using System;

namespace MyLib
{
    public static class MyRandom
    {
        // ゲーム中で唯一のインスタンス
        static Random random;

        // 初期化（シード指定無し）
        public static void Init()
        {
            random = new Random();
        }

        // 初期化（シードを指定）
        public static void Init(int seed)
        {
            random = new Random(seed);
        }

        // 指定した範囲の整数の乱数を取得する（maxは出ないので注意）
        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }

        // 指定した範囲の小数の乱数を取得する（maxは出ないので注意）
        public static float Range(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        // 指定した確率（％）でtrueになる
        public static bool Percent(float probability)
        {
            return random.NextDouble() * 100 < probability;
        }

        // 指定した範囲で乱数を返却する。
        // 例えば1.5fを指定すると、-1.5～+1.5の範囲の値を返却する。
        public static float PlusMinus(float value)
        {
            return Range(-value, value);
        }
    }
}
