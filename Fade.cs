using Raylib_cs;
using System.Numerics;

namespace Stairs
{
    class Fade : Object
    {
        public float fadeScale = 2f;
        public Texture2D fadeImage;
        public Vector2 Position = new(0, 0);
        public int Height;
        public int Width;

        public override void Load()
        {
            string basePath;

            if (OperatingSystem.IsMacOS()) basePath = Path.Combine(AppContext.BaseDirectory, "..", "Resources");
            else if (OperatingSystem.IsWindows()) basePath = AppContext.BaseDirectory;
            else basePath = AppContext.BaseDirectory;

            string fadeImagePath = Path.Combine(basePath, "assets", "fade.png");
            fadeImage = Raylib.LoadTexture(fadeImagePath);
            Raylib.SetTextureFilter(fadeImage, TextureFilter.Point);

            Width = fadeImage.Width;
            Height = fadeImage.Height;
        }

        public override void Draw()
        {
            Raylib.DrawTextureEx(fadeImage, Position, 0, fadeScale, Color.White);
        }

        public override void Update() { }
        public override void End() => Raylib.UnloadTexture(fadeImage);
    }
}