using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Vale.TileEngine;

namespace Vale.Components
{
    public class GameObject
    {
        protected GameObject()
        {
            this.Name = "n/a";
            this.Position = new Vector2(0, 0);
            this.Properties = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public static List<GameObject> FromXElement(TileMap map, XElement element)
        {
            List<GameObject> retVal = new List<GameObject>();
            foreach (XElement xObject in element.Elements("object"))
            {
                GameObject gameObject = new GameObject();
                gameObject.Name = (string)xObject.Attribute("name");
                gameObject.Type = (string)xObject.Attribute("type");
                int x = ((int)xObject.Attribute("x")) / map.TileWidth;
                int y = ((int)xObject.Attribute("y")) / map.TileHeight;
                gameObject.Position = new Vector2(x, y);
                var properties = xObject.Elements("properties");
                foreach (XElement property in properties.Elements("property"))
                {
                    gameObject.Properties.Add((string)property.Attribute("name"), (string)property.Attribute("value"));
                }
                retVal.Add(gameObject);
            }
            return retVal;
        }
    }
}
