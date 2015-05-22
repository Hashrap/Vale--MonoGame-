using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.TileEngine
{
    public class TileSet
    {
        public TileSet()
        {
        }

        public Texture2D Image { get; set; }
        public int FirstGid { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public Tile[] Tiles { get; set; }

        public void LoadFromElement(XElement xTileset, ContentManager content)
        {
            this.FirstGid = (int)xTileset.Attribute("firstgid");
            this.TileWidth = (int)xTileset.Attribute("tilewidth");
            this.TileHeight = (int)xTileset.Attribute("tileheight");
            string name = (string)xTileset.Attribute("name");
            this.Image = content.Load<Texture2D>("Tilesets/" + name);

            // Create the tileset
            this.Width = this.Image.Width / this.TileWidth;
            this.Height = this.Image.Height / this.TileHeight;
            this.Tiles = new Tile[this.Width * this.Height];

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    var id = x + y;
                    Tile tile = new Tile(this, id);
                    this.Tiles[id] = tile;
                }
            }
        }
    }
}

