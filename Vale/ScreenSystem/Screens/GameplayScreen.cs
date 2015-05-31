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
        Hero player;
        Texture2D cursorTexture;

        public GameplayScreen()
        {
            player = new Hero(this);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            cursorTexture = content.Load<Texture2D>("Art/cursor10x10.png");
            player.LoadContent();

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
            player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            SpriteBatch.Draw(cursorTexture, Vale.Control.Input.Instance.MousePosition, Color.White);
            player.Draw(gameTime);

            base.Draw(gameTime);

        }
    }
}
