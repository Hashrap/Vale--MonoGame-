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
        MapManager map;
        Hero player;
        Texture2D cursorTexture;

        public GameplayScreen()
        {
            player = new Hero(this);
            map = new MapManager(this);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            cursorTexture = content.Load<Texture2D>("Art/cursor10x10.png");
            player.LoadContent();
            map.LoadContent();

            Generator gen = new Generator(0, "test");
            gen.Cave(1, "222111", 40, 100, 100);
            map.Import(gen.exportVale(0));

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
            map.Draw(gameTime);
            player.Draw(gameTime);
            SpriteBatch.Draw(cursorTexture, Vale.Control.Input.Instance.MousePosition, Color.White);
            base.Draw(gameTime);
        }
    }
}
