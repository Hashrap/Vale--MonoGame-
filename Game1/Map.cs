//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGen
{
    /*This class is a parent class*/
    public class Map
    {
        /*these attributes are common to all map types.  Dimensions,
         * an array to hold tiles, and the flag enumeration for basic tile types.*/
        public Item[] items;
        public Tile[,] board;
        public List<Monster> monsters = new List<Monster>();
        protected MersenneTwister rng = new MersenneTwister();

        //g + b types are used to mark tiles as valid/invalid
        //for various purposes.  Disjoints, items, monsters, etc.
        [FlagsAttribute]
        public enum Tile { Wall = 0x0, Floor = 0x1, Valid = 0x2, ValidFloor = 0x4};

        public struct Point
        {
            private int x;
            private int y;

            public int X { get; set; }
            public int Y { get; set; }

            public Point(int _x, int _y) : this()
            {
                x = _x;
                y = _y;
            }
        }

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
            if (load == true)
                items = new Item[rng.Next(Convert.ToInt32((Size_X + Size_Y) / 15))];
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
                        case 0:
                            Console.Write('#');
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        /*Starts at a source tile, then spreads throughout adjacent floor
         * tiles and marks them reachable if they are set to unreachable.*/
        public int[,] exportMap()
        {
            int[,] map = new int[board.GetLength(0), board.GetLength(1)];
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = (int)board[x, y];
                }
            }
            return map;
        }
        /* Starts at a source tile, then spreads throughout adjacent floor
         * tiles and marks them reachable if they are set to unreachable.*/
        public void floodSearch(int y, int x)
        {
            if ((board[y, x] & Tile.Floor) == Tile.Floor)
            {
                if ((board[y, x] & Tile.Valid) != Tile.Valid)
                    board[y, x] |= Tile.Valid;
                else
                    // we've marked this tile already
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
        /* Starts at a source tile, then spreads throughout adjacent floor
         * tiles and marks them reachable if they are set to unreachable.
         * Not susceptible to stack overflow on large maps. */
        public void iterativeFloodSearch(int y, int x)
        {
            Stack<Point> searchStack = new Stack<Point>();
            HashSet<Point> searched = new HashSet<Point>();
            searchStack.Push(new Point(x, y));
            while (searchStack.Count > 0)
            {
                Point pos = searchStack.Pop();
                // check popped coordinates
                if ((board[pos.Y, pos.X] & Tile.Floor) == Tile.Floor)
                {
                    if ((board[pos.Y, pos.X] & Tile.Valid) != Tile.Valid)
                    {
                        board[pos.Y, pos.X] |= Tile.Valid;
                    }
                }
                // Log this coordinate as checked
                searched.Add(pos);

                // push adjacent coordinates
                List<Point> adjacents = new List<Point>();
                Point adjacent = new Point(pos.Y, pos.X - 1);
                adjacents.Add(adjacent);
                adjacent = new Point(pos.Y, pos.X + 1);
                adjacents.Add(adjacent);
                adjacent = new Point(pos.Y - 1, pos.X);
                adjacents.Add(adjacent);
                adjacent = new Point(pos.Y + 1, pos.X);
                adjacents.Add(adjacent);

                // Only check each tile once
                foreach (Point point in adjacents)
                {
                    if (!searched.Contains(point))
                        searchStack.Push(point);
                }
            }
        }

        /* Checks to see if floodFill() could reach every floor tile.
         * returns true if it reached every floor tile and false
         * if the map was disjointed and floodFill() missed a tile*/
        public bool isEverythingReachable()
        {
            badCount = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if ((board[i, j] & Tile.Floor) == Tile.Floor)
                        badCount++;
                }
            }
            if (badCount > 0)
                return false;
            else
                return true;
        }

        /* Randomly checks the map until it finds a floor tile,
         * then sends its index in the array back.  This
         * provides the starting tile for floodFill().*/

        //TODO: pick from near the center rather than randomly
        public int[] findFloor()
        {
            int x, y;
            while(true)
            {
                x = rng.Next(1, Size_X - 1);
                y = rng.Next(1, Size_Y - 1);
                if ((board[x, y] & Tile.Floor) == Tile.Floor)
                    return new int[2] { x, y };
            }
            return null; //If this happens something went wrong
        }
        /* Changes any unreachable floor into a wall*/
        public void fill()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    // Change unreachble floors to walls
                    if ((board[i, j] & Tile.Floor) == Tile.Floor)
                            board[i, j] &= ~Tile.Floor;
                }
            }
        }
        /* Sets all tiles to "invalid" */
        public void setInvalid()
        {
            //Sets every tile to 'bad', or 'unreachable'
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] &= ~Tile.Valid;
                }
            }
        }

        /* Sets all tiles to walls */
        public void setWall()
        {
            //goes through each row
            for (int i = 0; i < Size_Y; i++)
            {
                //goes through each column in a row
                for (int j = 0; j < Size_X; j++)

                    board[i, j] = Tile.Wall;
            }
        }
        /*This method keeps making duplicates
         * it should be threaded so we can pause it
         * and get unique items each time*/
        public void placeObjects()
        {
            int num = rng.Next(Convert.ToInt32((Size_X + Size_Y) / 20), Convert.ToInt32((Size_X + Size_Y) / 15));
            items = new Item[num];
            for (int i = 0; i < items.Length; i++)
            {
                bool place = false;
                //If it finds a bad spot, keep trying! 
                while (place == false)
                {
                    objectY = rng.Next(1, Size_Y - 1);
                    objectX = rng.Next(1, Size_X - 1);
                    if ((board[objectY, objectX] & Tile.Floor) == Tile.Floor)
                    {
                        //generate an item
                        items[i] = new Item(objectY, objectX, false);
                        place = true;
                    }
                }
            }
            for (int i = 0; i < num; i++)
            {
                bool place = false;
                while (place == false)
                {
                    objectY = rng.Next(1, Size_Y - 1);
                    objectX = rng.Next(1, Size_X - 1);
                    if ((board[objectY, objectX] & Tile.Floor) == Tile.Floor)
                    {
                        //generate a monster
                        monsters.Add(new Monster(objectX, objectY, false));
                        place = true;
                    }
                }
            }
            setInvalid();
        }
    }
}
