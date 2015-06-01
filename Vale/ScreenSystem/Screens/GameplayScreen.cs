using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DungeonGen;
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

            Generator gen = new Generator(0, "test");
            gen.Cave(1, "2222211", 40, 1000, 1000);
            gen.arrayOfMaps[0].printMap();

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
            player.Draw(gameTime);
            SpriteBatch.Draw(cursorTexture, Vale.Control.Input.Instance.MousePosition, Color.White);
            base.Draw(gameTime);
        }
    }
}
