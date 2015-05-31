using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem.Screens
{
    public class BackgroundScreen : GameScreen
    {
        private const float LOGO_SCREEN_HEIGHT_RATIO = 0.25f;
        private const float LOGO_SCREEN_BORDER_RATIO = 0.0375f;
        private const float LOGO_WIDTH_HEIGHT_RATIO = 1.4f;

        ContentManager content;

        private Texture2D backgroundTexture;

        public BackgroundScreen() { }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            backgroundTexture = content.Load<Texture2D>("Common/titlescreen");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            //SpriteBatch.Draw(backgroundTexture, fullscreen, Color.Black);
        }
    }
}
