using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DungeonGen;
using Vale.GameObjects;
using Vale.GameObjects.Actors;

namespace Vale.ScreenSystem.Screens
{
    public class GameplayScreen : GameScreen
    {
        public ContentManager Content { get; private set; }
        Texture2D cursorTexture;
        public Camera camera;
        public MouseProvider MouseProvider { get; private set; }
        public KeyboardProvider KeyboardProvider { get; private set; }

        List<GameObject> objects = new List<GameObject>();
        List<GameObject> objectQueue = new List<GameObject>();

        /// <summary>
        /// Should load all of the content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (Content == null)
            {
                Content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            cursorTexture = Content.Load<Texture2D>("Art/cursor10x10.png");

            MouseProvider = new MouseProvider(this);
            KeyboardProvider = new KeyboardProvider(this);
            var map = new MapManager(this);
            var player = new Hero(this, MouseProvider, KeyboardProvider);

            AddObject(MouseProvider);
            AddObject(map);
            AddObject(player);

            foreach (var gameObject in objects)
            {
                gameObject.LoadContent(Content);
            }

            var gen = new Generator(0, "test");
            gen.Cave(1, "22211111", 40, 100, 100);
            map.Import(gen.exportVale(0));

            camera = new Camera(this, ScreenManager.Game.GraphicsDevice.Viewport, new Vector2(map.Width, map.Height));
            camera.SetTarget(player);

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            Content.Unload();
        }

        public void AddObject(GameObject gameObject)
        {
            objects.Add(gameObject);
        }

        public void RemoveObject(GameObject gameObject)
        {
            objects.Remove(gameObject);
            objectQueue.Remove(gameObject);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // TODO: Handle screen state

            // The queue is a copy of the stack
            objectQueue.Clear();
            foreach (var gameObject in objects)
            {
                objectQueue.Add(gameObject);
            }

            // We start popping things off the end until the queue is empty so
            // that if something is removed we don't get confused.
            while (objectQueue.Count > 0)
            {
                GameObject gameObj = objectQueue[objectQueue.Count - 1];
                objectQueue.RemoveAt(objectQueue.Count - 1);

                // Update the object
                gameObj.Update(gameTime);
            }

            camera.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Camera draw needs to go first
            camera.Draw(gameTime, SpriteBatch);

            foreach (var gameObj in objects)
            {
                gameObj.Draw(gameTime, SpriteBatch);
            }

            SpriteBatch.Draw(cursorTexture, MouseProvider.PointerPosition, Color.White);

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
