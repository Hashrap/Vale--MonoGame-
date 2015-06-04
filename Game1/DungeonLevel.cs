//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGen
{
    public class DungeonLevel : Map
    {
        private class Leaf
        {
            private readonly int _minimumLeafSize = 6;

            private int top, left, height, width;
            private Leaf leftChild, rightChild;
            private Rectangle room;
            private List<Point> hall;
        }

        private struct Rectangle
        {
            private int x, y, width, height;
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            public Rectangle(int top, int left, int width, int height) : this()
            {
                this.x = left;
                this.y = top;
                this.width = width;
                this.height = height;
            }

            public Rectangle(Point a, Point b) : this()
            {
                this.x = Math.Min(a.X, b.X);
                this.y = Math.Min(a.Y, b.Y);
                this.width = Math.Abs(a.X - b.X);
                this.height = Math.Abs(a.Y - b.Y);
            }
        }

        public DungeonLevel(int size_x, int size_y)
            : base(size_y, size_x, true)
        {
            board = new Tile[base.Size_X, base.Size_Y];
            for (int i = 0; i < base.Size_Y; i++)
            //goes through each row
            {
                for (int j = 0; j < base.Size_X; j++)
                //goes through each column in a row
                {
                    board[i, j] &= ~Tile.Floor;
                }
            }
        }
        
        public Tile[,] dungeonGen(int iterations, double min_position, double max_position, int minimum_area)
        {
            //TODO: BSP dungeon gen
            //Horizontal
            if (rng.Next(99) % 2 == 1)
            {

            }
            //Vertical
            else
            {
            }

            return null;
        }

        public Tile[,] carveRoom(Tile[,] array)
        {
            bool good = false;
            int x;
            int x_length;
            int y;
            int y_length;
            while (good == false)
            {
                Console.WriteLine("CARVIN'");
                for (int i = 0; i < array.GetLength(0); i++)
                //goes through each row
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    //goes through each column in a row
                    {
                        board[i, j] |= Tile.Valid;
                    }
                }
                y_length = rng.Next(array.GetLength(0));
                x_length = rng.Next(array.GetLength(1));
                y = rng.Next(array.GetLength(0) - y_length);
                x = rng.Next(array.GetLength(1) - x_length);
                if (x_length * y_length > 8)
                {
                    for (int i = y; i < y_length; i++)
                    //goes through each row
                    {
                        for (int j = x; j < x_length; j++)
                        //goes through each column in a row
                        {
                            array[i, j] &= ~Tile.Valid;
                            array[i, j] |= Tile.Floor;
                            good = true;
                        }
                    }
                }
            }
            return array;
        }
    }
}