using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DungeonGen;
using Vale.GameObjects.Actors;
using System.Diagnostics;
using Vale.Control;
using System.Text.RegularExpressions;

namespace Vale.ScreenSystem.Screens
{
    public class GameplayScreen : GameScreen
    {
        ContentManager content;
        MapManager map;
        Hero player;
        Texture2D cursorTexture;
        public Camera camera;

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

            camera = new Camera(this, ScreenManager.Game.GraphicsDevice.Viewport, new Vector2(map.Width, map.Height));
            camera.SetTarget(player);

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
            camera.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Camera draw needs to go first
            camera.Draw(gameTime, SpriteBatch);
            //SpriteBatch.Begin();
            map.Draw(gameTime);
            player.Draw(gameTime);
            SpriteBatch.Draw(cursorTexture, camera.ScreenToWorldCoords(Input.Instance.MousePosition), Color.White);

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
