using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vale.ScreenSystem
{
    /// <summary>
    /// The initial screen for the game uses the old arcade formula which
    /// attempts to be cinematic.
    /// </summary>
    public class SplashScreen : GameScreen
    {
        private const float LogoScreenHeightRatio = 4f / 6f;
        private const float LogoWidthHeightRatio = 1.4f;
        private ContentManager content;
        private Rectangle destination;
        private TimeSpan duration;
        private Texture2D valeLogoTexture;

        public SplashScreen(TimeSpan duration)
        {
            this.duration = duration;
            this.TransitionOffTime = TimeSpan.FromSeconds(2.0);
        }

        /// <summary>
        /// Loads graphics content for this screen. the background texture is
        /// rather large so we use our own local ContentManager to load it. This
        /// allows us to unload it before going from the menus into the game
        /// itself; wheras if we used the shared ContentManager provided by the
        /// Game class, the content would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (this.content == null)
            {
                this.content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            this.valeLogoTexture = this.content.Load<Texture2D>("Common/logo");
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            int rectHeight = (int)(viewport.Height * LogoScreenHeightRatio);
            int rectWidth = (int)(rectHeight * LogoWidthHeightRatio);
            int posX = viewport.Bounds.Center.X - rectWidth / 2;
            int posY = viewport.Bounds.Center.Y - rectHeight / 2;

            this.destination = new Rectangle(posX, posY, rectWidth, rectHeight);
        }

        /// <summary>
        /// Unload the graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            this.content.Unload();
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            if (input.KeyboardState.GetPressedKeys().Length > 0
                || input.GamePadState.IsButtonDown(Buttons.A | Buttons.Start | Buttons.Back)
                || input.MouseState.LeftButton == ButtonState.Pressed)
            {
                this.duration = TimeSpan.Zero;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.duration -= gameTime.ElapsedGameTime;
            if (this.duration <= TimeSpan.Zero)
            {
                ExitScreen();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.GraphicsDevice.Clear(Color.White);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(this.valeLogoTexture, this.destination, Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}
