using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Stairs
{
    public class Player : Object
    {
        public int posX = 1200;
        public float posY = 277.5f;
        public int frameCounter = 0;
        public int frameSpeed = 8;
        public const int LeftBound = 40;
        public const int RightBound = 1455 - 40;
        public const float MoveSpeed = 4f;
        public const float FloorImagePosYBase = 277f;
        public const float FloorImagePosYOffset = 16f;
        public const float FloorImagePosYUpper = 277f / 2 + 112f + 44f;
        public const float TextureScale = 7.5f;
        public const float DrawPosYOffset = 405f - 277 / 2;
        public const float FloorMoveStartX = 450f;
        public const float FloorMoveEndX = 1012.5f;
        public const float SoundTriggerStartX = 1150f;
        public const float SoundTriggerEndX = 1455f;
        public int currentTextureIndex = 0;
        public bool isIdle = true;
        public string currentLocation = "left";
        public bool shouldGoRight = false;
        public List<Texture2D> walkTextures = new();
        public List<Texture2D> idleTextures = new();
        public Sound sound;

        public override void Load()
        {
            string basePath;

            if (OperatingSystem.IsMacOS()) basePath = Path.Combine(AppContext.BaseDirectory, "..", "Resources");
            else if (OperatingSystem.IsWindows()) basePath = AppContext.BaseDirectory;
            else basePath = AppContext.BaseDirectory;

            for (int i = 1; i <= 11; i++)
            {
                string walkPath = Path.Combine(basePath, "assets", "player", "walk", $"{i}.png");
                walkTextures.Add(Raylib.LoadTexture(walkPath));
            }

            for (int i = 1; i <= 6; i++)
            {
                string idlePath = Path.Combine(basePath, "assets", "player", "idle", $"{i}.png");
                idleTextures.Add(Raylib.LoadTexture(idlePath));
            }

            string soundPath = Path.Combine(basePath, "assets", "knock.wav");
            sound = Raylib.LoadSound(soundPath);
        }

        public override void Draw()
        {
            Texture2D currentTexture;

            if (isIdle) currentTexture = idleTextures[currentTextureIndex % idleTextures.Count];
            else currentTexture = walkTextures[currentTextureIndex % walkTextures.Count];

            Rectangle source = new(0, 0, currentTexture.Width, currentTexture.Height);

            if (currentLocation == "left") source.Width = -currentTexture.Width;

            Rectangle dest = new(posX - Program.cameraX, DrawPosYOffset, currentTexture.Width * TextureScale, currentTexture.Height * TextureScale);
            Vector2 origin = new(currentTexture.Width * TextureScale / 2, currentTexture.Height * TextureScale / 2);

            Raylib.DrawTexturePro(currentTexture, source, dest, origin, 0, Color.White);
            Program.fade.Position = new Vector2(posX - Program.cameraX - Program.fade.fadeImage.Width / 2 * 2f, posY - DrawPosYOffset - 800);
        }

        public override void Update()
        {
            bool moving = false;

            float targetRotation = 0f;

            if (Program.front.isFinished)
            {
                if (Raylib.IsKeyDown(KeyboardKey.D))
                {
                    targetRotation = 3f;
                    currentLocation = "right";
                    posX += (int)MoveSpeed;
                    moving = true;

                    if (posX > FloorMoveStartX && posX < FloorMoveEndX)
                    {
                        if (shouldGoRight) Floor.ImagePosY += 2;
                        else Floor.ImagePosY -= 2;
                    }
                }
                else if (Raylib.IsKeyDown(KeyboardKey.A))
                {
                    targetRotation = -3f;
                    currentLocation = "left";
                    posX -= (int)MoveSpeed;
                    moving = true;

                    if (posX > FloorMoveStartX && posX < FloorMoveEndX)
                    {
                        if (!shouldGoRight) Floor.ImagePosY += 2;
                        else Floor.ImagePosY -= 2;
                    }
                }
                else
                {
                    targetRotation = 0f;
                }
            }

            float lerpSpeed = 0.05f;
            Program.CameraRotation = Program.Lerp(Program.CameraRotation, targetRotation, lerpSpeed);


            if (posX < FloorMoveStartX)
            {
                Floor.ImagePosY = (int)FloorImagePosYBase + (int)FloorImagePosYOffset - 2;
                shouldGoRight = true;
            }
            if (posX > FloorMoveEndX)
            {
                Floor.ImagePosY = 570;
                shouldGoRight = false;
            }

            if (posX > FloorMoveStartX && posX < FloorMoveEndX)
            {
                if (!Program.front.isFinished) return;
                if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.D))
                {
                    Program.shakeFrequencyX = 3f;
                    Program.shakeFrequencyY = 3f;
                }
                else
                {
                    Program.shakeFrequencyX = 0.3f;
                    Program.shakeFrequencyY = 0.3f;
                }
            }
            else
            {
                Program.shakeFrequencyX = 0.3f;
                Program.shakeFrequencyY = 0.3f;
            }

            frameCounter++;
            if (frameCounter >= frameSpeed)
            {
                frameCounter = 0;
                currentTextureIndex++;
            }

            if (posX > SoundTriggerStartX && posX < SoundTriggerEndX)
            {
                if (!Program.front.isFinished) return;
                E.isShowing = true;
                if (!Program.front.isFinished) return;
                if (Raylib.IsKeyPressed(KeyboardKey.E)) Raylib.PlaySound(sound);
            }
            else E.isShowing = false;

            if (posX < LeftBound) posX = LeftBound;
            if (posX > RightBound) posX = RightBound;

            isIdle = !moving;
        }

        public override void End()
        {
            foreach (Texture2D tex in walkTextures) Raylib.UnloadTexture(tex);
            foreach (Texture2D tex in idleTextures) Raylib.UnloadTexture(tex);
        }
    }
}
