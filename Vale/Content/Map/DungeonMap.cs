//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vale.Content.Map
{
    class DungeonMap : Map
    {
        private const int MINIMUM_ROOM_AREA = 9;
        private int rand;
        private int count;
        Tile[,] altBoard;
        private MersenneTwister rng = new MersenneTwister();
        public DungeonMap(int size_y, int size_x)
            : base(size_y, size_x, true)
        {
            board = new Tile[base.Size_Y, base.Size_X];
            altBoard = new Tile[base.Size_X, base.Size_Y];
            for (int i = 0; i < base.Size_Y; i++)
            //goes through each row
            {
                for (int j = 0; j < base.Size_X; j++)
                //goes through each column in a row
                {
                    board[i, j] |= Tile.W;
                }
            }
        }
        
        public Tile[,] dungeonGen(Tile[,] board, int iterations, double min_position, double max_position)
        {
            //TODO: BSP dungeon gen
            if (rng.Next(99) % 2 == 1)
            {

            }
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
                        board[i, j] |= Tile.W;
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
                            array[i, j] &= ~Tile.W;
                            array[i, j] |= Tile.f;
                            array[i, j] |= Tile.g;
                            good = true;
                        }
                    }
                }
            }
            return array;
        }
    }
}