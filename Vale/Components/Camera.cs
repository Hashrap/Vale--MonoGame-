using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.ScreenSystem;
using Vale.TileEngine;

namespace Vale.Components
{
    /// <summary>
    /// The Camera's draw method handles drawing the Character as well as the map
    /// in whatever locations are actually "visible".
    /// </summary>
    public class Camera
    {
        GameplayScreen gameScreen;

        // The viewport's dimensions need to be retrieved dynamically otherwise
        // strange things happen if the window is resized. Strange things happen
        // anyway but only if the window is shrunk below whatever size it needed to be.
        private int viewportWidth
        {
            get { return this.gameScreen.ScreenManager.GraphicsDevice.Viewport.Width; }
            set { }
        }
        private int viewportHeight
        {
            get { return this.gameScreen.ScreenManager.GraphicsDevice.Viewport.Height; }
            set { }
        }

        public Camera(GameplayScreen gameScreen)
        {
            this.gameScreen = gameScreen;
        }

        public Vector2 Position { get; set; }

        public void Update(GameTime gameTime)
        {
            // Center the camera on the player, if possible.
            Vector2 position = new Vector2();
            position.X = this.gameScreen.Player.Position.X - (this.viewportWidth / 2);
            position.X = MathHelper.Clamp(position.X, 0, this.gameScreen.GameMap.WorldWidth - this.viewportWidth);
            position.Y = this.gameScreen.Player.Position.Y - (this.viewportHeight / 2);
            position.Y = MathHelper.Clamp(position.Y, 0, this.gameScreen.GameMap.WorldHeight - this.viewportHeight);
            this.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw the map.
            // The camera has an absolute position.
            int max_y = ((int)(this.Position.Y + this.viewportHeight) / this.gameScreen.GameMap.TileHeight) + 1;
            int max_x = ((int)(this.Position.X + this.viewportWidth) / this.gameScreen.GameMap.TileWidth) + 1;
            max_y = (int)MathHelper.Clamp(max_y, 0, this.gameScreen.GameMap.TileMap.Height);
            max_x = (int)MathHelper.Clamp(max_x, 0, this.gameScreen.GameMap.TileMap.Width);

            int offset_x = (int)(this.Position.X + this.viewportWidth) % this.gameScreen.GameMap.TileMap.TileWidth;
            int offset_y = (int)(this.Position.Y + this.viewportHeight) % this.gameScreen.GameMap.TileMap.TileHeight;

            foreach (var layer in this.gameScreen.GameMap.TileMap.layers)
            {
                if (layer.Visible == false)
                {
                    continue;
                }
                Tile[,] tiles = layer.Tiles;
                Rectangle destination = new Rectangle(0 - offset_x, 0 - offset_y, this.gameScreen.GameMap.TileMap.TileWidth, this.gameScreen.GameMap.TileMap.TileHeight);
                for (int y = (int)(this.Position.Y / this.gameScreen.GameMap.TileMap.TileHeight); y < max_y; y++)
                {
                    for (int x = (int)(this.Position.X / this.gameScreen.GameMap.TileMap.TileWidth); x < max_x; x++)
                    {
                        Tile tile = tiles[y, x];
                        if (tile != null)
                        {
                            spriteBatch.Draw(tile.TileSet.Image, destination, tile.SourceRectangle, Color.White);
                        }
                        destination.X += this.gameScreen.GameMap.TileMap.TileWidth;
                    }
                    destination.Y += this.gameScreen.GameMap.TileMap.TileHeight;
                    destination.X = -offset_x;
                }
            }

            // Draw the character.
            this.gameScreen.Player.CurrentCharacter.Sprite.Position = this.gameScreen.Player.Position - this.Position;
            this.gameScreen.Player.CurrentCharacter.Sprite.Draw(spriteBatch, gameTime);

            // Draw the Npcs
            foreach (Npc npc in this.gameScreen.Npcs)
            {
                if (npc.Position.X >= this.Position.X 
                    && npc.Position.X <= this.Position.X + this.viewportWidth
                    && npc.Position.Y >= this.Position.Y 
                    && npc.Position.Y <= this.Position.Y + this.viewportHeight)
                {
                    npc.Sprite.Position = npc.Position - this.Position;
                    npc.Sprite.Draw(spriteBatch, gameTime);
                }
            }
        }
    }
}