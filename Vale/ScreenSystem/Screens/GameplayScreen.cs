using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DungeonGen;
using Vale.Control;
using Vale.GameObjects;
using Vale.GameObjects.Actors;
using Vale.GameObjects.Collision;

namespace Vale.ScreenSystem.Screens
{
    public class GameplayScreen : GameScreen
    {
        public ContentManager Content { get; private set; }
        Texture2D cursorTexture;
        public Camera camera;
        public ValeTree Actors { get; private set; }
        public MapManager Map { get; private set; }
        public MouseProvider MouseProvider { get; private set; }
        public KeyboardProvider KeyboardProvider { get; private set; }
        public UnitFactory UnitCreator;

        List<GameObject> objects = new List<GameObject>();
        List<GameObject> objectQueue = new List<GameObject>();
        
        private Texture2D WhiteTexture { get; set; }
        private bool DebugValeTree { get; set; }
        private bool DebugMap { get; set; }
        private readonly int spawnMin = 40;

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
            WhiteTexture = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1);
            WhiteTexture.SetData(new Color[] { Color.White });
            DebugValeTree = false;
            DebugMap = false;

            Actors = new ValeTree(this, new Vector2(20, 20), 5);
            MouseProvider = new MouseProvider(this);
            KeyboardProvider = new KeyboardProvider(this);
            Map = new MapManager(this);
            UnitCreator = new UnitFactory(this);
            AddObject(Actors);
            AddObject(MouseProvider);
            AddObject(KeyboardProvider);
            AddObject(Map);
            AddObject(UnitCreator.CreateUnit("unit_grunt", GameActor.Faction.Hostile, new Vector2(100, 100)));

            //Create a playspace
            var gen = new Generator(0, "test");
            gen.Cave(1, "2221111", 40, 100, 100);
            Map.Import(gen.exportVale(0));

            //Find a 40x40 spawn zone for the player to spawn in
            Random rng = new Random();
            Vector2 spawn = new Vector2(rng.Next(Map.Width - spawnMin), rng.Next(Map.Height - spawnMin));
            AABB spawnArea = new AABB(spawn, spawn + new Vector2(spawnMin, spawnMin));
            while (!Map.Query(spawnArea))
            {
                spawn = new Vector2(rng.Next(Map.Width), rng.Next(Map.Height));
                spawnArea = new AABB(spawn, spawn + new Vector2(spawnMin, spawnMin));
            }
            var player = new Hero(this, MouseProvider, KeyboardProvider, spawn + new Vector2(spawnMin/2, spawnMin/2));
            AddObject(player);

            camera = new Camera(this, ScreenManager.Game.GraphicsDevice.Viewport, new Vector2(Map.Width, Map.Height));
            camera.SetTarget(player);

            foreach (var gameObject in objects)
            {
                gameObject.LoadContent(Content);
            }

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

            foreach (GameActor actor in Actors.GetAllObjects())
            {
                
            }

            camera.Update(gameTime);

            if (Input.Instance.KeyPress('c'))
                DebugValeTree = !DebugValeTree;
            if (Input.Instance.KeyPress('m'))
                DebugMap = !DebugMap;
        }

        public override void Draw(GameTime gameTime)
        {
            // Camera draw needs to go first
            camera.Draw(gameTime, SpriteBatch);

            foreach (var gameObj in objects)
            {
                gameObj.Draw(gameTime, SpriteBatch);
            }

            if (DebugValeTree)
                Actors.DebugDraw(WhiteTexture, SpriteBatch);
            if (DebugMap)
                Map.DebugDraw(WhiteTexture, SpriteBatch);
            
            SpriteBatch.Draw(cursorTexture, MouseProvider.PointerPosition, Color.White);

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
