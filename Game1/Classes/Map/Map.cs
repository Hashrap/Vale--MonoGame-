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
        public enum Tile { Wall = 0x0, Walkable = 0x1, Valid = 0x2};

        public struct Point
        {

            public int X { get; private set; }
            public int Y { get; private set; }

            public Point(int _x, int _y) : this()
            {
                X = _x;
                Y = _y;
            }
        }

        //properties
        public int BadCount { get ; private set; }
        public int Size_X { get; private set; }
        public int Size_Y { get; private set; }

        private int objectX;
        private int objectY;
        //Constructor
        public Map(int x, int y, bool load)
        {
            Size_Y = y;
            Size_X = x;
            if (load == true)
                items = new Item[rng.Next(Convert.ToInt32((Size_X + Size_Y) / 15))];
        }
        //Iterates through the array and
        //prints each tile out into the console
        public void printMap()
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    switch ((int)board[x, y])
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
        public void floodSearch(int x, int y)
        {
            if ((board[x, y] & Tile.Walkable) == Tile.Walkable)
            {
                if ((board[x, y] & Tile.Valid) != Tile.Valid)
                    board[x, y] |= Tile.Valid;
                else
                    // we've marked this tile already
                    return;
            }
            else
                return;
            /*Recursively propagates to each square around it.  Currently spawns in
             * the surrounding cardinal squares.*/
            floodSearch(x, y + 1);
            floodSearch(x, y - 1);
            floodSearch(x + 1, y);
            floodSearch(x - 1, y);
        }
        /* Starts at a source tile, then spreads throughout adjacent floor
         * tiles and marks them reachable if they are set to unreachable.
         * Not susceptible to stack overflow on large maps. */
        public void iterativeFloodSearch(int x, int y)
        {
            Stack<Point> searchStack = new Stack<Point>();
            HashSet<Point> searched = new HashSet<Point>();
            searchStack.Push(new Point(x, y));
            while (searchStack.Count > 0)
            {
                Point pos = searchStack.Pop();
                // check popped coordinates
                if ((board[pos.X, pos.Y] & Tile.Valid) != Tile.Valid)
                {
                    board[pos.X, pos.Y] |= Tile.Valid;
                }

                // Log this coordinate as checked
                searched.Add(pos);

                // push adjacent coordinates
                List<Point> adjacents = new List<Point>();
                Point adjacent = new Point(pos.X, pos.Y - 1);
                adjacents.Add(adjacent);
                adjacent = new Point(pos.X, pos.Y + 1);
                adjacents.Add(adjacent);
                adjacent = new Point(pos.X - 1, pos.Y);
                adjacents.Add(adjacent);
                adjacent = new Point(pos.X + 1, pos.Y);
                adjacents.Add(adjacent);

                foreach (Point point in adjacents)
                {
                    if (!searched.Contains(point) && (board[point.X, point.Y] & Tile.Walkable) == Tile.Walkable)
                        searchStack.Push(point);
                }
            }
        }

        /* Checks to see if floodFill() could reach every floor tile.
         * returns true if it reached every floor tile and false
         * if the map was disjointed and floodFill() missed a tile*/
        public bool isEverythingReachable()
        {
            BadCount = 0;
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if ((board[x, y] & Tile.Walkable) == Tile.Walkable &&
                        (board[x, y] & Tile.Valid) != Tile.Valid)
                        BadCount++;
                }
            }
            if (BadCount > 0)
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
                if ((board[x, y] & Tile.Walkable) == Tile.Walkable)
                    return new int[2] { x, y };
            }
            return null; //If this happens something went wrong
        }
        /* Changes any unreachable floor into a wall*/
        public void Fill()
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    // Change unreachble floors to walls
                    if ((board[x, y] & Tile.Walkable) == Tile.Walkable &&
                        (board[x, y] & Tile.Valid) != Tile.Valid)
                            board[x, y] &= ~Tile.Walkable;
                }
            }
        }
        /* Removes Valid flag from all tiles */
        public void RemoveValid()
        {
            //Sets every tile to 'bad', or 'unreachable'
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    board[x, y] &= ~Tile.Valid;
                }
            }
        }

        /* Sets all tiles to walls */
        public void setWall()
        {
            //goes through each row
            for (int x = 0; x < Size_Y; x++)
            {
                //goes through each column in a row
                for (int y = 0; y < Size_X; y++)

                    board[x, y] = Tile.Wall;
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
                    if ((board[objectX, objectY] & Tile.Walkable) == Tile.Walkable)
                    {
                        //generate an item
                        items[i] = new Item(objectX, objectY, false);
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
                    if ((board[objectX, objectY] & Tile.Walkable) == Tile.Walkable)
                    {
                        //generate a monster
                        monsters.Add(new Monster(objectX, objectY, false));
                        place = true;
                    }
                }
            }
            RemoveValid();
        }
    }
}
