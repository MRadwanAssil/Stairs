using Raylib_cs;
using System.Numerics;

namespace Stairs
{
    class Front : Object
    {
        public static Texture2D frontImage1;
        public static Texture2D frontImage2;

        private float timer = 0f;
        private float fadeDuration = 1f;
        private float visibleDuration1 = 3.5f;
        private float visibleDuration2 = 7f;

        private float alpha2 = 1f;
        private float alpha1 = 1f;

        public bool isFinished = false;

        public override void Load()
        {
            string basePath;

            if (OperatingSystem.IsMacOS()) basePath = Path.Combine(AppContext.BaseDirectory, "..", "Resources");
            else if (OperatingSystem.IsWindows()) basePath = AppContext.BaseDirectory;
            else basePath = AppContext.BaseDirectory;

            frontImage1 = Raylib.LoadTexture(Path.Combine(basePath, "assets", "up.png"));
            frontImage2 = Raylib.LoadTexture(Path.Combine(basePath, "assets", "job.png"));
        }

        public override void Update()
        {
            if (isFinished) return;

            timer += Raylib.GetFrameTime();

            if (timer > visibleDuration1 && timer < visibleDuration1 + fadeDuration)
            {
                float t = (timer - visibleDuration1) / fadeDuration;
                alpha2 = 1f - t;
            }
            else if (timer >= visibleDuration1 + fadeDuration)
            {
                alpha2 = 0f;
            }

            if (timer > visibleDuration2 && timer < visibleDuration2 + fadeDuration)
            {
                float t = (timer - visibleDuration2) / fadeDuration;
                alpha1 = 1f - t;
            }
            else if (timer >= visibleDuration2 + fadeDuration)
            {
                alpha1 = 0f;
            }

            if (timer >= visibleDuration2 + fadeDuration)
            {
                isFinished = true;
            }
        }

        public override void Draw()
        {
            if (alpha1 > 0f)
            {
                Raylib.DrawTexture(frontImage1, 0, 0, new Color(255, 255, 255, (int)(alpha1 * 255)));
            }

            if (alpha2 > 0f)
            {
                Raylib.DrawTexture(frontImage2, 0, 0, new Color(255, 255, 255, (int)(alpha2 * 255)));
            }
        }

        public override void End()
        {
            Raylib.UnloadTexture(frontImage1);
            Raylib.UnloadTexture(frontImage2);
        }
    }
}
