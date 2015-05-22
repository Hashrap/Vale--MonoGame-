using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Vale.Components;

namespace Vale.ScreenSystem
{
    public class GameplayScreen : GameScreen
    {
        private string savePath;

        public List<Npc> Npcs;

        public GameplayScreen(string saveFilePath)
            : base()
        {
            this.savePath = saveFilePath;
            System.IO.FileStream saveFileStream = null;
            try
            {
                saveFileStream = System.IO.File.OpenRead(this.savePath);
                this.SaveExists = true;
                saveFileStream.Close();
            } catch (System.IO.FileNotFoundException)
            {
                this.SaveExists = false;
            }

            this.GameMap = new GameMap();
            this.Player = new Player(this);
            this.CollisionDetector = new CollisionDetector(this);
            this.Npcs = new List<Npc>();
        }

        public Camera Camera { get; set; }
        public GameMap GameMap { get; set; }
        public Player Player { get; set; }
        public CollisionDetector CollisionDetector { get; set; }

        public bool SaveExists { get; protected set; }

        public override void LoadContent()
        {
            base.LoadContent();
            this.GameMap.Load(this.ScreenManager.Content, "Introduction.tmx");
            this.Player.LoadContent(this.ScreenManager.Content);
            this.Player.Position = this.GameMap.GetPlayerSpawns()[0].Position;
            this.Player.Position = new Vector2(this.Player.Position.X * this.GameMap.TileWidth, this.Player.Position.Y * this.GameMap.TileHeight);
            this.Camera = new Camera(this);

            foreach(GameObject gameObject in this.GameMap.GetNpcSpawns()) {
                Npc npc = new Npc(this);
                npc.Position = gameObject.Position;
                npc.Position = new Vector2(npc.Position.X * this.GameMap.TileWidth, npc.Position.Y * this.GameMap.TileHeight);
                npc.LoadContent(this.ScreenManager.Content, "Npc1");
                this.Npcs.Add(npc);
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            base.HandleInput(input, gameTime);
            // TODO: Handle Pause, Open Menu, etc.
            this.Player.HandleInput(input);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            this.GameMap.Update(gameTime);
            this.CollisionDetector.Update(gameTime);
            this.Player.Update(gameTime);
            foreach (Npc npc in this.Npcs)
            {
                npc.Update(gameTime);
            }
            this.Camera.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.ScreenManager.SpriteBatch.Begin();
            this.Camera.Draw(ScreenManager.SpriteBatch, gameTime);
            this.ScreenManager.SpriteBatch.End();
        }
    }
}