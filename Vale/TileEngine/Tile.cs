using Microsoft.Xna.Framework;

namespace Vale.TileEngine
{
    public class Tile
    {
        private static int tileId = 0;

        public Tile(TileSet tileset, int id)
        {
            this.TileSet = tileset;
            int width = tileset.Image.Width / tileset.TileWidth;
            int height = tileset.Image.Height / tileset.TileHeight;
            int x = id % width;
            int y = id / width;
            this.SourceRectangle = new Rectangle(x * tileset.TileWidth, y * tileset.TileHeight, tileset.TileWidth, tileset.TileHeight);
            this.TileId = Tile.tileId++;
        }

        public int TileId;
        public TileSet TileSet { get; set; }
        public Rectangle BoundingBox { get; set; }
        public Rectangle SourceRectangle { get; set; }
    }
}

