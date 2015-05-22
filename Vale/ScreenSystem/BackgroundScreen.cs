using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem
{
    /// <summary>
    /// The background screen sits behind all the other screens. It draws a
    /// background image that remains fixed in place regardless of whatever
    /// transitions the screens on top of it may be doing.
    /// </summary>
    public class BackgroundScreen : GameScreen
    {
        private const float LogoScreenHeightRatio = 0.25f;
        private const float LogoScreenBorderRatio = 0.0375f;
        private const float LogoWidthHeightRatio = 1.4f;
        private Texture2D backgroundTexture;
        private Rectangle logoDestination;
        private Texture2D logoTexture;
        private Rectangle viewport;

        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            this.logoTexture = ScreenManager.Content.Load<Texture2D>("Common/logo");
            this.backgroundTexture = ScreenManager.Content.Load<Texture2D>("Common/titlescreen");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 logoSize = new Vector2();
            logoSize.Y = viewport.Height * LogoScreenHeightRatio;
            logoSize.X = logoSize.Y * LogoWidthHeightRatio;
            float border = viewport.Height * LogoScreenBorderRatio;
            Vector2 logoPosition = new Vector2(viewport.Width - border - logoSize.X, viewport.Height - border - logoSize.Y);
            this.logoDestination = new Rectangle((int)logoPosition.X, (int)logoPosition.Y, (int)logoSize.X, (int)logoSize.Y);
            this.viewport = viewport.Bounds;
        }

        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen. This
        /// override forces the coveredByOtherScreen parameter to false in order
        /// to stop the base Update method from wanting to transition off.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            //base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(this.backgroundTexture, this.viewport, Color.White);
            //ScreenManager.SpriteBatch.Draw(this.logoTexture, this.logoDestination, Color.White * 0.6f);
            ScreenManager.SpriteBatch.End();
        }
    }
}
