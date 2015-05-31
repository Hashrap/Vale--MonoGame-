//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vale.Content.Map
{
    /*This class is a parent class*/
    class Map
    {
        /*these attributes are common to all map types.  Dimensions,
         * an array to hold tiles, and the flag enumeration for basic tile types.*/
        //public Item[] items;
        public Tile[,] board;
        //public List<Monster> monsters = new List<Monster>();
        private MersenneTwister rng = new MersenneTwister();

        //g + b types are used to mark tiles as valid/invalid
        //for various purposes.  Disjoints, items, monsters, etc.
        [FlagsAttribute]
        public enum Tile { f = 0x1, W = 0x2, g = 0x4, b = 0x8 };

        //properties
        private int badCount;
        public int BadCount
        {
            get { return badCount; }
            set { badCount = value; }
        }

        private int size_x;
        public int Size_X { get { return size_x; } }
        private int size_y;
        public int Size_Y { get { return size_y; } }

        private int objectX;
        private int objectY;
        //Constructor
        public Map(int y, int x, bool load)
        {
            size_y = y;
            size_x = x;
            //if (load == true)
                //items = new Item[rng.Next(Convert.ToInt32((Size_X + Size_Y) / 15))];
        }
        //Iterates through the array and
        //prints each tile out into the console
        public void printMap()
        {
            for (int k = 0; k < board.GetLength(0); k++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    switch ((int)board[k, j])
                    {
                        //changes floor tiles for the symbol '.'
                        case 1:
                            Console.Write('.');
                            break;
                        //changes wall tiles into '#'
                        case 2:
                            Console.Write('#');
                            break;
                        //down stairs into '>'
                        case 9:
                            Console.Write('>');
                            break;
                        //up stairs into '<'
                        case 5:
                            Console.Write('<');
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        /*Starts at a source tile, then spreads throughout adjacent floor
         * tiles and marks them reachable if they are set to unreachable.*/
        public void floodSearch(int y, int x)
        {
            if ((board[y, x] & Tile.f) == Tile.f)
            {
                if ((board[y, x] & Tile.b) == Tile.b)
                {
                    board[y, x] |= Tile.g;
                    board[y, x] &= ~Tile.b;
                }
                else
                    return;
            }
            else
                return;
            /*Recursively propagates to each square around it.  Currently spawns in
             * the surrounding cardinal squares.*/
            floodSearch(y, x + 1);
            floodSearch(y, x - 1);
            floodSearch(y + 1, x);
            floodSearch(y - 1, x);
        }
        /* Checks to see if floodFill() could reach every floor tile.
         * returns true if it reached every floor tile and false
         * if the map was disjointed and floodFill() missed a tile*/
        public bool isEverythingReachable()
        {
            BadCount = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if ((board[i, j] & Tile.b) == Tile.b)
                    {
                        if ((board[i, j] & Tile.f) == Tile.f)
                        {
                            /* While it's at it, it counts the unreachable
                             * floor tiles.  If the number is too large,
                             * a map should be discarded*/
                            badCount++;
                        }
                    }
                }
            }
            if (badCount > 0)
            {
                return false;
            }
            else
                return true;
        }
        /*Iterates through the map until it finds a floor tile,
         * then sends its index in the array back.  This
         * provides the starting tile for floodFill().*/
        public int[] findFloor()
        {
            int[] index = new int[2];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if ((board[i, j] & Tile.f) == Tile.f)
                    {
                        index[0] = i;
                        index[1] = j;
                        return index;
                    }
                }
            }
            return index; //If this happens something went wrong
        }
        /* Changes any unreachable floor into a wall*/
        public void fill()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if ((board[i, j] & Tile.f) == Tile.f)
                    {
                        if ((board[i, j] & Tile.b) == Tile.b)
                        {
                            board[i, j] |= Tile.W;
                            board[i, j] &= ~Tile.f;
                        }
                    }
                }
            }
        }
        /* Sets all tiles to "unreachable"*/
        public void floodPrep()
        {
            //Sets every tile to 'bad', or 'unreachable'
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] |= Tile.b;
                }
            }
        }
        public void clean()
        {
            //Turns off g + b enums
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] &= ~Tile.b;
                    board[i, j] &= ~Tile.g;
                }
            }
        }
        /*This method keeps making duplicates
         * it should be threaded so we can pause it
         * and get unique items each time*/
        /*public void placeObject()
        {
            for (int i = 0; i < items.Length; i++)
            {
                bool place = false;
                //If it finds a bad spot, keep trying! 
                while (place == false)
                {
                    objectY = rng.Next(1, Size_Y - 1);
                    objectX = rng.Next(1, Size_X - 1);
                    if ((board[objectY, objectX] & Tile.f) == Tile.f)
                    {
                        //generate an item
                        items[i] = new Item(objectY, objectX, false);
                        place = true;
                    }
                }
            }
            for (int i = 0; i < rng.Next(Convert.ToInt32((Size_X + Size_Y) / 20), Convert.ToInt32((Size_X + Size_Y) / 15)); i++)
            {
                bool place = false;
                while (place == false)
                {
                    objectY = rng.Next(1, Size_Y - 1);
                    objectX = rng.Next(1, Size_X - 1);
                    if ((board[objectY, objectX] & Tile.f) == Tile.f)
                    {
                        //generate a monster
                        monsters.Add(new Monster(objectX, objectY, false));
                        place = true;
                    }
                }
            }
            clean();
            bool good = false;
            bool alt = false;
            //If it finds a bad spot, keep trying
            // TODO: does not check if good spots exist, so the program hangs on small/filled maps.
            while (good == false)
            {
                objectY = rng.Next(1, Size_Y - 1);
                objectX = rng.Next(1, Size_X - 1);
                if (alt == false && (int)board[objectY, objectX] == 1)
                {
                    //f|b can be used to create down stairs I suppose
                    board[objectY, objectX] |= Tile.b;
                    alt = true;
                }
                if (alt == true && (int)board[objectY,objectX] == 1)
                {
                    //f|g can be up stairs
                    board[objectY, objectX] |= Tile.g;
                    good = true;
                }
            }
        }*/
    }
}
