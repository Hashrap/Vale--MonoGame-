using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;
using Vale.Components;

namespace Vale.TileEngine
{
    public enum MapOrientation
    {
        Orthogonal,
        Isometric,
        Staggered,
    }

    public class TileMap
    {
        public List<TileMapLayer> layers;
        public List<TileSet> tilesets;
        public List<GameObject> gameObjects;
        public ContentManager Content;

        public TileMap(ContentManager content)
        {
            this.Content = content;
            this.layers = new List<TileMapLayer>();
            this.tilesets = new List<TileSet>();
            this.gameObjects = new List<GameObject>();
        }

        public string Version { get; private set; }
        public MapOrientation Orientation { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public void Load(string contentPath)
        {
            Stream mapFileStream = File.OpenRead(this.Content.RootDirectory + "/" + contentPath);
            XElement xMap = XElement.Load(mapFileStream);
            if (xMap.Name != "map" || !xMap.HasElements)
            {
                throw new Exception("Tried to load a file that does not contain map data.");
            }

            this.Version = (string)xMap.Attribute("Version");
            switch ((string)xMap.Attribute("orientation"))
            {
                case "isometric":
                    this.Orientation = MapOrientation.Isometric;
                    break;
                case "staggered":
                    this.Orientation = MapOrientation.Staggered;
                    break;
                default:
                case "orthogonal":
                    this.Orientation = MapOrientation.Orthogonal;
                    break;
            }

            this.Width = (int)xMap.Attribute("width");
            this.Height = (int)xMap.Attribute("height");
            this.TileWidth = (int)xMap.Attribute("tilewidth");
            this.TileHeight = (int)xMap.Attribute("tileheight");
            foreach (XElement element in xMap.Elements())
            {
                switch(element.Name.LocalName)
                {
                    case "tileset":
                        TileSet tileset = new TileSet();
                        tileset.LoadFromElement(element, Content);
                        this.tilesets.Add(tileset);
                        break;
                    case "layer":
                        TileMapLayer toAdd = TileMapLayer.FromElement(this, element);
                        this.layers.Add(toAdd);
                        break;
                    case "objectgroup":
                        this.gameObjects.AddRange(GameObject.FromXElement(this, element));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}