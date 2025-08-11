using Raylib_cs;
using System.Numerics;
using System;

namespace Stairs
{
    class Program
    {
        public static int ScreenHeight = 555;
        public static int ScreenWidth = 750;
        public static float cameraX = 1455 - 750;
        public static float targetX = cameraX;
        public static float cameraSmoothSpeed = 5f;
        public static float minCameraX = -60f;
        public static float maxCameraX = 1515f - ScreenWidth;
        public static float shakeAmplitude = 5f;
        public static float shakeFrequencyX = 0.3f;
        public static float shakeFrequencyY = 0.3f;
        public static float delta;
        public static float shakeTimer = 0f;
        public static float shakeOffsetX;
        public static float shakeOffsetY;
        public static float ShakeRandomRange = 0.2f;
        public static float ShakeRandomOffset = 0.1f;
        public static float CameraRotation = 0f;
        public static float CameraZoom = 1f;
        public static float TwoPi = 2f * MathF.PI;
        public static int TargetFPS = 60;
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
        public static Floor floor = new();
        public static Fade fade = new();
        public static Player player = new();
        public static Front front = new();
        public static Random rnd = new();
        public static Camera2D camera = new()
        {
            Target = new Vector2(0, 0),
            Offset = new Vector2(0, 0),
            Rotation = CameraRotation,
            Zoom = CameraZoom
        };

        static void Main()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Stairs");
            Raylib.InitAudioDevice();
            Raylib.SetTargetFPS(TargetFPS);

            floor.Load();
            fade.Load();
            player.Load();
            front.Load();
            E.Load();

            while (!Raylib.WindowShouldClose())
            {
                delta = Raylib.GetFrameTime();

                targetX = player.posX - ScreenWidth / 2;
                cameraX = Lerp(cameraX, targetX, cameraSmoothSpeed * delta);

                if (cameraX < minCameraX) cameraX = minCameraX;
                if (cameraX > maxCameraX) cameraX = maxCameraX;

                shakeTimer += delta;

                shakeOffsetX = shakeAmplitude * MathF.Sin(shakeTimer * shakeFrequencyX * TwoPi);
                shakeOffsetY = shakeAmplitude * MathF.Cos(shakeTimer * shakeFrequencyY * TwoPi);

                shakeOffsetX += (float)(rnd.NextDouble() * ShakeRandomRange - ShakeRandomOffset);
                shakeOffsetY += (float)(rnd.NextDouble() * ShakeRandomRange - ShakeRandomOffset);

                camera.Target = new Vector2(0, 0);
                camera.Offset = new Vector2(shakeOffsetX, shakeOffsetY);
                camera.Zoom = CameraZoom;
                camera.Rotation = CameraRotation;

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.BeginMode2D(camera);

                floor.Update();
                floor.Draw();
                player.Update();
                front.Update();
                E.Update(delta);
                front.Draw();
                fade.Draw();
                if (front.isFinished) E.Draw();

                Raylib.EndMode2D();
                Raylib.EndDrawing();
            }

            floor.End();
            front.End();
            player.End();
            Raylib.CloseWindow();
        }
    }
}
