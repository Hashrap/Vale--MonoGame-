using System.Xml.Linq;

namespace Vale.TileEngine
{
    public class TileMapLayer
    {
        const uint FlippedHorizontallyFlag = 0x80000000;
        const uint FlippedVerticallyFlag = 0x40000000;
        const uint FlippedDiagonallyFlag = 0x20000000;

        public TileMapLayer(TileMap map, int width, int height)
        {
            this.TileMap = map;
            this.Width = width;
            this.Height = height;
            this.Data = new uint[height, width];
            this.Tiles = new Tile[height, width];
        }

        public TileMap TileMap { get; set; }
        public string Name { get; set; }
        public double Opacity { get; set; }
        public bool Visible { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public uint [,] Data { get; set; }
        public Tile[,] Tiles { get; set; }

        public static TileMapLayer FromElement(TileMap map, XElement xLayer)
        {
            TileMapLayer layer = new TileMapLayer(map, (int)xLayer.Attribute("width"), (int)xLayer.Attribute("height"));
            layer.Name = (string)xLayer.Attribute("name");
            if (xLayer.Attribute("opacity") != null)
            {
                layer.Opacity = (double)xLayer.Attribute("opacity");
            } else
            {
                // default
                layer.Opacity = 1.0;
            }

            if (xLayer.Attribute("visible") != null)
            {
                layer.Visible = (int)xLayer.Attribute("visible") == 1;
            } else
            {
                // default
                layer.Visible = true;
            }

            if (xLayer.HasElements)
            {
                XElement data = xLayer.Element("data");
                int count = 0;
                foreach (XElement tile in data.Elements())
                {
                    // sort of a tricksey way of pretending you're iterating using nested loops
                    // y is the "outer" loop value, x is the "inner" loop value
                    int x = count % layer.Width;
                    int y = count / layer.Width;
                    uint gid = (uint)tile.Attribute("gid");

                    bool flippedHorizontally = (gid & FlippedHorizontallyFlag) != 0;
                    bool flippedVertically = (gid & FlippedVerticallyFlag) != 0;
                    bool flippedDiagonally = (gid & FlippedDiagonallyFlag) != 0;

                    gid &= ~(FlippedHorizontallyFlag | FlippedVerticallyFlag | FlippedDiagonallyFlag);
                    for (var i = map.tilesets.Count - 1; i >= 0; i--)
                    {
                        TileSet tileset = map.tilesets[i];
                        if (tileset.FirstGid <= gid)
                        {
                            layer.Tiles[y, x] = tileset.Tiles[((int)gid) - tileset.FirstGid];
                            break;
                        }
                    }

                    count++;
                }
            }

            return layer;
        }
    }
}