using System;
using System.Diagnostics;
using System.Threading;
using DxLibDLL;

namespace Shooting
{
    class Application
    {
        const int TargetFPS = 60; // 目標のFPS(Frame Per Second, 1秒あたりのフレーム数)
        static readonly bool EnableFrameSkip = true; // 高負荷時にフレームスキップするか（falseの場合は処理落ち（スロー））
        const double MaxAllowSkipTime = 0.2; // フレームスキップする最大間隔（秒）。これ以上の間隔が空いた場合はスキップせずに処理落ちにする。
        const long IntervalTicks = (long)(1.0 / TargetFPS * 10000000); // フレーム間のTick数。1Tick = 100ナノ秒 = 1/10000000秒
        const int MaxAllowSkipCount = (int)(TargetFPS * MaxAllowSkipTime);

        static long nextFrameTicks = IntervalTicks; // 次のフレームの目標時刻
        static Stopwatch stopwatch = new Stopwatch(); // FPS制御のために時間を計るための高精度タイマー
        static int skipCount = 0; // 何回連続でフレームスキップしたか
        static long fpsTicks = 0; // FPS計測のためのTicks。
        static int fpsFrameCount = 0; // FPS計測のためのフレームカウント。60回数えるごとに、要した時間からFPSを算出する。

        /// <summary>
        /// 現在のFPS（Frame per Second）
        /// </summary>
        public static float CurrentFPS { get; private set; }

        static Game game;

        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest; // スレッドの優先度を上げておく
            // 画面リフレッシュレートと目標フレームレートが等しい場合は垂直同期を有効に、等しくない場合は垂直同期を無効にする
            DX.SetWaitVSyncFlag(DX.GetRefreshRate() == TargetFPS ? DX.TRUE : DX.FALSE);
            DX.SetWindowText("シューティングゲーム"); // ウィンドウのタイトル
            DX.SetGraphMode(Screen.Width, Screen.Height, 32); // ウィンドウサイズ（画面解像度）の指定
            DX.ChangeWindowMode(DX.TRUE); // ウィンドウモードにする（DX.FALSEを指定するとフルスクリーンになる）
            DX.SetAlwaysRunFlag(DX.TRUE); // ウィンドウが非アクティブでも動作させる

            DX.DxLib_Init(); // DXライブラリの初期化

            DX.SetMouseDispFlag(DX.TRUE); // マウスを表示する（DX.FALSEを指定すると非表示になる）
            DX.SetDrawScreen(DX.DX_SCREEN_BACK); // 描画先を裏画面とする（ダブルバッファ）

            game = new Game();
            game.Init();

            DX.ScreenFlip();
            stopwatch.Start();

            while (DX.ProcessMessage() == 0) // ウィンドウが閉じられるまで繰り返す
            {
                // FPSの計測
                fpsFrameCount++;
                if (fpsFrameCount >= 60)
                {
                    long elapsedTicks = stopwatch.Elapsed.Ticks - fpsTicks;
                    float elapsedSec = elapsedTicks / 10000000f;
                    CurrentFPS = fpsFrameCount / elapsedSec;

                    fpsFrameCount = 0;
                    fpsTicks = stopwatch.Elapsed.Ticks;
                }

                game.Update();

                if (DX.GetWaitVSyncFlag() == DX.TRUE)
                {
                    if (EnableFrameSkip)
                    {
                        long waitTicks = nextFrameTicks - stopwatch.Elapsed.Ticks; // 余った時間

                        if (waitTicks < 0) // 目標時刻をオーバーしている
                        {
                            if (skipCount < MaxAllowSkipCount) // 連続フレームスキップ数が最大スキップ数を超えていなければ
                            {
                                skipCount++; // フレームスキップ（描画処理を飛ばす）
                            }
                            else
                            {
                                // 最大スキップ数を超えてるので、フレームスキップしないで描画
                                nextFrameTicks = stopwatch.Elapsed.Ticks;
                                Draw();
                            }
                        }
                        else
                        {
                            Draw();
                        }

                        nextFrameTicks += IntervalTicks;
                    }
                    else
                    {
                        Draw();
                    }
                }
                else
                {
                    long waitTicks = nextFrameTicks - stopwatch.Elapsed.Ticks; // 余った時間（待機が必要な時間）

                    if (EnableFrameSkip && waitTicks < 0)
                    {
                        if (skipCount < MaxAllowSkipCount)
                        {
                            skipCount++; // フレームスキップ（描画処理を飛ばす）
                        }
                        else
                        {
                            nextFrameTicks = stopwatch.Elapsed.Ticks;
                            Draw();
                        }
                    }
                    else
                    {
                        if (waitTicks > 20000) // あと2ミリ秒以上待つ必要がある
                        {
                            // Sleep()は指定した時間でピッタリ戻ってくるわけではないので、
                            // 余裕を持って、「待たなければならない時間-2ミリ秒」Sleepする
                            int waitMillsec = (int)(waitTicks / 10000) - 2;
                            Thread.Sleep(waitMillsec);
                        }

                        // 時間が来るまで何もしないループを回して待機する
                        while (stopwatch.Elapsed.Ticks < nextFrameTicks)
                        {
                        }

                        Draw();
                    }
                    nextFrameTicks += IntervalTicks;
                }
            }

            DX.DxLib_End(); // DXライブラリ終了処理
        }

        static void Draw()
        {
            DX.ClearDrawScreen(); // 描画先の内容をクリアする
            game.Draw(); // ゲーム描画
            DX.ScreenFlip(); // 裏画面と表画面を入れ替える
            skipCount = 0; // フレームスキップのカウントをリセット
        }
    }
}
