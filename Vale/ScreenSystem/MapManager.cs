using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem
{
    public class MapManager
    {
        [FlagsAttribute]
        public enum State { Wall = 0x0, Walkable = 0x1 }
        public struct Tile
        {
            public Vector2 Position { get; set; }

            // width and height are static - assumes all tiles have equal size
            public static int Width { get; set; }
            public static int Height { get; set; }

            private State _state;
            public State State { get; set; }
        }

        private Tile[,] _map;
        public int Size_X { get; private set; }
        public int Size_Y { get; private set; }

        private Texture2D floor;
        private Texture2D wall;

        public Vale.ScreenSystem.GameScreen GameScreen { get; set; }

        public MapManager(GameScreen gs)
        {
            GameScreen = gs;
        }

        public void LoadContent()
        {
            ContentManager content = new ContentManager(GameScreen.ScreenManager.Game.Services, "Content"); // TODO : what the fuck goes where 'null' is?
            floor = content.Load<Texture2D>("Art/whsq20x20.png");
            wall = content.Load<Texture2D>("Art/bksq20x20.png");
            Tile.Height = 20;
            Tile.Width = 20;
        }

        public void Load(string filename)
        {
            Console.WriteLine("File map loading not implemented");
        }

        public void Import(int[,] map)
        {
            Size_X = map.GetLength(0);
            Size_Y = map.GetLength(1);

            _map = new Tile[Size_X, Size_Y];

            for(int x = 0; x < Size_X; x++)
            {
                for(int y = 0; y < Size_Y; y++)
                {
                    _map[x, y].State = (State)map[x, y];
                    _map[x, y].Position = new Vector2(Tile.Width * x, Tile.Height * y);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int x = 0; x < Size_X; x++)
            {
                for (int y = 0; y < Size_Y; y++)
                {
                    switch(_map[x,y].State)
                    {
                        case State.Walkable:
                            GameScreen.SpriteBatch.Draw(floor,_map[x,y].Position,Color.White);
                            break;
                        case State.Wall:
                            GameScreen.SpriteBatch.Draw(wall, _map[x, y].Position, Color.White);
                            break;
                    }
                }
            }
        }
    }
}
