using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Vale.GameObjects.Actors;

namespace Vale.ScreenSystem.Screens
{
    public class GameplayScreen : GameScreen
    {
        ContentManager content;
        Texture2D cursorTexture;

        public GameplayScreen()
        {
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            cursorTexture = content.Load<Texture2D>("Art/cursor10x10.png");
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // TODO: Handle screen state

            // Delegate to the components
        }

        public override void Draw(GameTime gameTime)
        {

            SpriteBatch.Draw(cursorTexture, Vale.Control.Input.Instance.MousePosition, Color.White);

            base.Draw(gameTime);

        }
    }
}
