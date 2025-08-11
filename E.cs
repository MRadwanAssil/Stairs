using Raylib_cs;
using System.Numerics;

namespace Stairs
{
    class E
    {
        public static bool isShowing = true;
        public static Texture2D eImage;
        public static float opacity = 0f;
        public const float OpacityChangeSpeed = 2f;
        public static readonly Vector2 Position = new(10, Program.ScreenHeight - 38);
        public const float Scale = 1.75f;
        public const byte ColorChannelMax = 255;

        public static void Load()
        {
            string basePath;

            if (OperatingSystem.IsMacOS()) basePath = Path.Combine(AppContext.BaseDirectory, "..", "Resources");
            else if (OperatingSystem.IsWindows()) basePath = AppContext.BaseDirectory;
            else basePath = AppContext.BaseDirectory;

            string eImagePath = Path.Combine(basePath, "assets", "E.png");
            eImage = Raylib.LoadTexture(eImagePath);
            Raylib.SetTextureFilter(eImage, TextureFilter.Point);
        }

        public static void Update(float deltaTime)
        {
            if (isShowing)
            {
                if (opacity < 1f)
                {
                    opacity += deltaTime * OpacityChangeSpeed;
                    if (opacity > 1f) opacity = 1f;
                }
            }
            else
            {
                if (opacity > 0f)
                {
                    opacity -= deltaTime * OpacityChangeSpeed;
                    if (opacity < 0f) opacity = 0f;
                }
            }
        }

        public static void Draw()
        {
            if (opacity > 0f)
            {
                Color colorWithOpacity = new Color(ColorChannelMax, ColorChannelMax, ColorChannelMax, (byte)(opacity * ColorChannelMax));
                Raylib.DrawTextureEx(eImage, Position, 0, Scale, colorWithOpacity);
            }
        }
    }
}
