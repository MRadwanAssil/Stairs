using Raylib_cs;
using System.Numerics;

namespace Stairs
{
    class Floor : Object
    {
        public static float scale = 7.5f;
        public static int tileHeight;
        public static int ImagePosY = 0;
        public static int offsetY;
        public static int yToRender = 0;
        public const int HalfScreenHeight = 277 / 2;

        public static Texture2D floorImage;
        public static Texture2D stairImage;

        public override void Load()
        {
            string basePath;

            if (OperatingSystem.IsMacOS()) basePath = Path.Combine(AppContext.BaseDirectory, "..", "Resources");
            else if (OperatingSystem.IsWindows()) basePath = AppContext.BaseDirectory;
            else basePath = AppContext.BaseDirectory;

            string floorPath = Path.Combine(basePath, "assets", "floor.png");
            string stairPath = Path.Combine(basePath, "assets", "stair.png");

            floorImage = Raylib.LoadTexture(floorPath);
            stairImage = Raylib.LoadTexture(stairPath);

            Raylib.SetTextureFilter(floorImage, TextureFilter.Point);
            Raylib.SetTextureFilter(stairImage, TextureFilter.Point);

            tileHeight = (int)(floorImage.Height * scale);
        }

        public override void Draw()
        {
            if (ImagePosY >= tileHeight) ImagePosY -= tileHeight;
            if (ImagePosY <= -tileHeight) ImagePosY += tileHeight;

            Raylib.DrawTextureEx(floorImage, new Vector2(0 - Program.cameraX, yToRender), 0, scale, Color.White);
            Raylib.DrawTextureEx(floorImage, new Vector2(0 - Program.cameraX, yToRender - tileHeight), 0, scale, Color.White);
            Raylib.DrawTextureEx(floorImage, new Vector2(0 - Program.cameraX, yToRender + tileHeight), 0, scale, Color.White);

            if (Program.player.shouldGoRight) Program.player.Draw();

            Raylib.DrawTextureEx(stairImage, new Vector2(0 - Program.cameraX, yToRender), 0, scale, Color.White);
            Raylib.DrawTextureEx(stairImage, new Vector2(0 - Program.cameraX, yToRender - tileHeight), 0, scale, Color.White);
            Raylib.DrawTextureEx(stairImage, new Vector2(0 - Program.cameraX, yToRender + tileHeight), 0, scale, Color.White);

            if (!Program.player.shouldGoRight) Program.player.Draw();
        }

        public override void Update()
        {
            if (ImagePosY >= tileHeight) ImagePosY -= tileHeight;
            if (ImagePosY <= -tileHeight) ImagePosY += tileHeight;

            yToRender = ImagePosY - HalfScreenHeight;
        }

        public override void End()
        {
            Raylib.UnloadTexture(floorImage);
            Raylib.UnloadTexture(stairImage);
        }
    }
}
