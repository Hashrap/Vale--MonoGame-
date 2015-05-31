using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem.Screens
{
    public class SplashScreen : GameScreen
    {
        private const float LOGO_SCREEN_HEIGHT_RATIO = 4f / 6f;
        private const float LOGO_WIDTH_HEIGHT_RATIO = 1.4f;

        private Rectangle destination;
        private TimeSpan duration;
        private Texture2D valeLogoTexture;
        private ContentManager content;

        public SplashScreen(TimeSpan duration)
        {
            this.duration = duration;
            //this.TransitionOffTime = TimeSpan.FromSeconds(2.0);
        }

        public override void LoadContent()
        {
            System.Diagnostics.Debug.WriteLine("LoadContent()");
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            valeLogoTexture = content.Load<Texture2D>("Common/logo");
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            int rectHeight = (int)(viewport.Height * LOGO_SCREEN_HEIGHT_RATIO);
            int rectWidth = (int)(rectHeight * LOGO_WIDTH_HEIGHT_RATIO);
            int posX = viewport.Bounds.Center.X - rectWidth / 2;
            int posY = viewport.Bounds.Center.Y - rectHeight / 2;

            this.destination = new Rectangle(posX, posY, rectWidth, rectHeight);
        }

        public override void UnloadContent()
        {
            System.Diagnostics.Debug.WriteLine("UnloadContent()");
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            duration -= gameTime.ElapsedGameTime;
            if (duration <= TimeSpan.Zero)
            {
                ScreenManager.Add(new GameplayScreen());
                Exit();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(this.valeLogoTexture, this.destination, Color.White);
        }
    }
}
