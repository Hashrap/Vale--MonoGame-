//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGen
{
    public class DungeonLevel : Map
    {
        private class Node
        {
            internal readonly int _minimumLeafSize;

            internal Rectangle Bounds;
            internal Node leftChild, rightChild;
            internal Rectangle room;
            internal List<Point> hall;
        }

        private struct Rectangle
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Top { get { return Y; } }
            public int Left { get { return X; } }
            public int Width { get; private set; }
            public int Height { get; private set; }

            public Rectangle(int top, int left, int width, int height) : this()
            {
                this.X = left;
                this.Y = top;
                this.Width = width;
                this.Height = height;
            }

            public Rectangle(Point a, Point b) : this(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y))
            {
            }
        }

        public DungeonLevel(int size_x, int size_y)
            : base(size_y, size_x, true)
        {
            board = new Tile[base.Size_X, base.Size_Y];
            for (int x = 0; x < base.Size_Y; x++)
            //goes through each row
            {
                for (int y = 0; y < base.Size_X; y++)
                //goes through each column in a row
                {
                    board[x, y] &= ~Tile.Walkable;
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
                            array[i, j] |= Tile.Walkable;
                            good = true;
                        }
                    }
                }
            }
            return array;
        }
    }
}