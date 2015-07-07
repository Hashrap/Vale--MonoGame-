using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.ScreenSystem.Screens;

using Vale.GameObjects;
using Vale.GameObjects.Collision;
using System.Security.Cryptography;

namespace Vale.GameObjects
{
    public class MapManager : GameObject
    {
        [FlagsAttribute]
        ///<summary>
        ///     Bitfield for storing tile properties
        ///</summary>
        public enum State
        {
            Wall = 0x0,
            Walkable = 0x1
        }

        /// <summary>
        ///     Base unit of each map
        /// </summary>
        public struct Tile
        {
            //width and height are static - assumes all tiles have equal size
            /// </summary>
            ///     Width of the tile texture
            /// </summary>
            public static short Width { get; internal set; }
            /// </summary>
            ///     Height of the tile texture
            /// </summary>
            public static short Height { get; internal set; }

            /// <summary>
            ///     Bitfield for storing additional tile info
            /// </summary>
            public State State { get; set; }
        }

        const int TILE_WIDTH = 30;
        const int TILE_HEIGHT = 30;

        // 2D array of Tile objects representing the playable area
        private Tile[,] _map;

        /// <summary>
        ///     Gets the Tile X dimension of the current map
        /// </summary>
        public int Size_X { get; private set; }
        /// <summary>
        ///     Gets the Tile Y dimension of the current map
        /// </summary>
        public int Size_Y { get; private set; }

        /// <summary>
        ///     Gets the total pixel X dimension of the current map
        /// </summary>
        public int Width { get { return Size_X * TILE_WIDTH; } private set { } }
        /// <summary>
        ///     Gets the total pixel Y dimension of the current map
        /// </summary>
        public int Height { get { return Size_Y * TILE_HEIGHT; } private set { } }

        private Texture2D floor;
        private Texture2D wall;

        public GameplayScreen GameScreen { get; set; }

        public MapManager(GameplayScreen gs)
            : base(gs)
        {
            GameScreen = gs;
        }

        /// <summary>
        ///     Loads assets
        /// </summary>
        public override void LoadContent(ContentManager content)
        {
            floor = content.Load<Texture2D>("Art/whsq20x20.png");
            wall = content.Load<Texture2D>("Art/bksq20x20.png");
            Tile.Height = TILE_WIDTH;
            Tile.Width = TILE_HEIGHT;
        }

        ///<summary> 
        ///     Loads a map from a save file
        ///</summary>
        ///<param name="filename">Path to save file</param>
        public void Load(string filename)
        {
            Console.WriteLine("File map loading not implemented");
        }

        ///<summary>
        ///     Loads a map from the Generator's export() method
        ///</summary>
        ///<param name="map">2D array of ints representing map tiles</param>
        public void Import(int[,] map)
        {
            Size_X = map.GetLength(0);
            Size_Y = map.GetLength(1);

            _map = new Tile[Size_X, Size_Y];

            for (int x = 0; x < Size_X; x++)
            {
                for (int y = 0; y < Size_Y; y++)
                {
                    _map[x, y].State = (State)map[x, y];
                }
            }
        }

        public bool Query(int x, int y)
        {
            return _map[x, y].State == State.Walkable;
        }

        public bool Query(Vector2 pos)
        {
            return Query(((int)(pos.X / TILE_WIDTH)), ((int)(pos.Y / TILE_HEIGHT)));
        }

        public bool Query(AABB bounds)
        {
            return Query(bounds.Origin) && Query(bounds.Opposite);
        }

        /// <summary>
        /// Is called every graphical frame
        /// </summary>
        /// <param name="gameTime">GameTime information</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Size_X; x++)
            {
                for (int y = 0; y < Size_Y; y++)
                {
                    switch (_map[x, y].State)
                    {
                        case State.Walkable:
                            // FIXME: Use a correctly sized asset
                            spriteBatch.Draw(floor, new Vector2(Tile.Width*x, Tile.Height*y), null, Color.White, 0, Vector2.Zero, 30f / 20f, SpriteEffects.None, 0);
                            break;
                        case State.Wall:
                            // FIXME: Use a correctly sized asset
                            spriteBatch.Draw(wall, new Vector2(Tile.Width*x, Tile.Height*y), null, Color.White, 0, Vector2.Zero, 30f / 20f, SpriteEffects.None, 0);
                            break;
                    }
                }
            }
        }
    }
}
