using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace Vale.ScreenSystem
{
    public class FrameRateCounter : DrawableGameComponent
    {
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private NumberFormatInfo format;
        private int frameCounter;
        private int frameRate;
        private Vector2 position;
        private ScreenManager screenManager;

        public FrameRateCounter(ScreenManager screenManager)
            : base(screenManager.Game)
        {
            this.screenManager = screenManager;
            this.format = new NumberFormatInfo();
            this.format.NumberDecimalSeparator = ".";
            this.position = new Vector2(30, 25);
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime;
            if (this.elapsedTime <= TimeSpan.FromSeconds(1))
            {
                return;
            }
            this.elapsedTime -= TimeSpan.FromSeconds(1);
            this.frameRate = this.frameCounter;
            this.frameCounter = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            this.frameCounter++;
            string fps = string.Format(this.format, "{0} fps", this.frameRate);
            this.screenManager.SpriteBatch.Begin();
            this.screenManager.SpriteBatch.DrawString(this.screenManager.Fonts.FrameRateCounterFont, fps, this.position + Vector2.One, Color.Black);
            this.screenManager.SpriteBatch.DrawString(this.screenManager.Fonts.FrameRateCounterFont, fps, this.position, Color.White);
            this.screenManager.SpriteBatch.End();
        }
    }
}
