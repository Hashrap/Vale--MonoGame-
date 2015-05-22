using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.TileEngine;

namespace Vale.Components
{
    public class GameMap
    {
        public static int tileWidth = 16;
        public static int tileHeight = 16;
        public int TileWidth { get { return GameMap.tileWidth; } }
        public int TileHeight { get { return GameMap.tileHeight; } }
        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }
        public Rectangle[,] CollisionLayer { get; set; }

        public TileMap TileMap { get; set; }

        public GameMap()
        {
        }

        public Rectangle Viewport { get; set; }

        public void Load(ContentManager content, string mapPath)
        {
            this.TileMap = new TileMap(content);
            TileMap.Load(mapPath);
            foreach (var gameObject in this.TileMap.gameObjects)
            {
                if (gameObject.Type == "PlayerSpawn")
                {
                    System.Diagnostics.Debug.WriteLine("Spawn point for the player at : " + gameObject.Position);
                }
                else if (gameObject.Type == "NpcSpawn")
                {
                    System.Diagnostics.Debug.WriteLine("Spawn point for an NPC at: " + gameObject.Position);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Dumping gameObject: " + gameObject);
                }
            }

            // Build the collision layer (i.e. Add bounding boxes).
            this.CollisionLayer = new Rectangle[this.TileMap.Height, this.TileMap.Width];
            TileMapLayer cTemp =  this.TileMap.layers.Find(x => x.Name == "Collision");
            if (cTemp == null)
            {
                throw new System.Exception("Invalid map file! There's no collision layer.");
            }

            for (int y = 0; y < cTemp.Height; y++)
            {
                for (int x = 0; x < cTemp.Width; x++)
                {
                    if (cTemp.Tiles[y, x] != null)
                    {
                        this.CollisionLayer[y, x] = new Rectangle(x * this.TileMap.TileWidth, y * this.TileMap.TileHeight, this.TileMap.TileWidth, this.TileMap.TileHeight);
                    }
                }
            }

            GameMap.tileWidth = this.TileMap.TileWidth;
            GameMap.tileHeight = this.TileMap.TileHeight;
            this.WorldWidth = TileMap.Width * TileMap.TileWidth;
            this.WorldHeight = TileMap.Height * TileMap.TileHeight;
        }

        /// <summary>
        /// Gets a list of all of the Player spawn points in the level.
        /// </summary>
        /// <returns>List of GameObjects which a player can spawn from.</returns>
        public List<GameObject> GetPlayerSpawns()
        {
            return this.TileMap.gameObjects.FindAll(x => x.Type == "PlayerSpawn");
        }

        public List<GameObject> GetNpcSpawns()
        {
            return this.TileMap.gameObjects.FindAll(x => x.Type == "NpcSpawn");
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var layer in this.TileMap.layers)
            {
                Tile[,] tiles = layer.Tiles;
                Rectangle destination = new Rectangle(0, 0, TileMap.TileWidth, TileMap.TileHeight);
                for (int y = 0; y < layer.Height; y++)
                {
                    for (int x = 0; x < layer.Width; x++)
                    {
                        Tile tile = tiles[y, x];
                        if (tile != null)
                        {
                            destination.X = x * TileMap.TileWidth;
                            destination.Y = y * TileMap.TileHeight;
                            spriteBatch.Draw(tile.TileSet.Image, destination, tile.SourceRectangle, Color.White);
                        }
                    }
                }
            }
        }
    }
}