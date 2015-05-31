//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vale.Content.Map
{
    class Generator
    {
        //Attributes and objects
        public enum DungeonType { Cave, Dungeon, Forest };
        public DungeonType type;
        public Map[] arrayOfLevels;
        public string name;
        //Constructor
        public Generator(int itype, string name)
        {
            switch (itype)
            {
                case 0:
                    type = DungeonType.Cave;
                    break;
                case 1:
                    type = DungeonType.Dungeon;
                    break;
                case 2:
                    type = DungeonType.Forest;
                    break;
            }
            this.name = name;
        }

        public Generator() { }

        public void Cave(int levels, int algorithm1, int algorithm2, int wall, int size_x, int size_y)
        {
            arrayOfLevels = new CaveMap[levels];
            for (int i = 0; i < levels; i++)
            {
                CaveMap cl;
                bool good = false;
                while (good != true)
                {
                    cl = new CaveMap(size_y, size_x);
                    cl.randomFill(wall);

                    for (int j = 0; j < algorithm1; j++) //ages the cells using the 4-5 | 2 rule
                        cl.ageDungeon(2);
                    for (int j = 0; j < algorithm2; j++)//ages the cells using the 4-5 rule
                        cl.ageDungeon(1);
                    arrayOfLevels[i] = cl;

                    /*These check the validity of the map, trashes "bad" maps until it gets a good one
                    * Will attempt to repair mildly disjointed rooms.*/
                    arrayOfLevels[i].floodPrep();
                    int[] fTile = arrayOfLevels[i].findFloor();
                    arrayOfLevels[i].floodSearch(fTile[0], fTile[1]);
                    if (arrayOfLevels[i].isEverythingReachable() == false && arrayOfLevels[i].BadCount > size_x + size_y)
                    {
                        good = false; //Trashes the map
                    }
                    else if (arrayOfLevels[i].isEverythingReachable() == false && arrayOfLevels[i].BadCount <= size_x + size_y)
                    {
                        arrayOfLevels[i].fill(); //Repairs the map
                        good = true;
                    }
                    else
                        good = true; //Map is fine as is
                    //arrayOfLevels[i].placeObject();
                    int waiter = 0;
                    while (waiter < 10000000)
                    {
                        waiter++; //prevents duplicates (using the same seed twice)
                    }
                }
            }
        }

        public void Dungeon(int levels, double pos_min, double pos_max, int min_room, int iterations, int size_y, int size_x)
        {
            arrayOfLevels = new DungeonMap[levels];
            for (int i = 0; i < levels; i++)
            {
                DungeonMap dl = new DungeonMap(size_y, size_x);
                dl.dungeonGen(dl.board, iterations, pos_min, pos_max);
                arrayOfLevels[i] = dl;
            }
        }
        public void Forest(int size_y, int size_x)
        {
            //Code to generate a forest
        }

        //This should save the dungeon/items/monsters.  Not the character or inventory.
        public void saveInstance()
        {
            StreamWriter sr = new StreamWriter("Content/txt/" + name + ".txt");
            sr.WriteLine(name);
            sr.WriteLine(type);
            sr.WriteLine(arrayOfLevels.Length);
            for (int i = 0; i < arrayOfLevels.Length; i++)
            {
                sr.WriteLine(i + 1);
                sr.WriteLine(arrayOfLevels[i].Size_Y + " " + arrayOfLevels[i].Size_X);
                for (int k = 0; k < arrayOfLevels[i].board.GetLength(0); k++)
                {
                    for (int j = 0; j < arrayOfLevels[i].board.GetLength(1); j++)
                    {
                        sr.Write((int)arrayOfLevels[i].board[k, j] + "|");
                    }
                    sr.WriteLine();
                }
                /*sr.WriteLine("[Items]");
                sr.WriteLine(arrayOfLevels[i].items.Length);
                foreach (Item item in arrayOfLevels[i].items)
                    sr.WriteLine(item.ToString(true));
                sr.WriteLine("[Monsters]");
                sr.WriteLine(arrayOfLevels[i].monsters.Count());
                foreach (Monster monster in arrayOfLevels[i].monsters)
                    sr.WriteLine(monster.ToString(true));*/
            }
            sr.Close();
        }
        public void loadInstance(string name)  //Loads the dungeon.
        {
            string[] data;  //for string.Split()'s
            StreamReader sr = new StreamReader("Content/txt/" + name + ".txt");
            this.name = sr.ReadLine();//load the name of the instance
            if (sr.ReadLine().Equals("Cave")) //load the type of instance
            {
                type = DungeonType.Cave;
            }
            else
            {
                type = DungeonType.Dungeon;
            }
            arrayOfLevels = new CaveMap[int.Parse(sr.ReadLine())]; //load # of levels
            for (int i = 0; i < arrayOfLevels.Length; i++)
            {
                sr.ReadLine(); //skip a line
                data = sr.ReadLine().Split(' '); //Load the dimensions into the array
                //Create a new CaveLevel of the saved dimensions
                CaveMap cl = new CaveMap(int.Parse(data[0]), int.Parse(data[1]));
                //and copy it into the array
                arrayOfLevels[i] = cl;
                for (int k = 0; k < arrayOfLevels[i].board.GetLength(0); k++)
                {
                    data = sr.ReadLine().Split('|');
                    for (int j = 0; j < data.Length - 1; j++)
                    {
                        //Copies the actual map from the file
                        arrayOfLevels[i].board[k, j] = (Map.Tile)int.Parse(data[j]);
                    }
                }
                sr.ReadLine();//Skip a line
                //Make a new array of items
                /*arrayOfLevels[i].items = new Item[int.Parse(sr.ReadLine())];
                for (int k = 0; k < arrayOfLevels[i].items.Length; k++)
                {
                    data = sr.ReadLine().Split('|');//AND MY AXE
                    //Make a new item at the saved location
                    Item item = new Item(int.Parse(data[12]), int.Parse(data[11]), true);
                    //and then copy it into the array and set attributes from the file
                    arrayOfLevels[i].items[k] = item;
                    arrayOfLevels[i].items[k].Name = data[0];
                    arrayOfLevels[i].items[k].AttackRatingAdd = int.Parse(data[1]);
                    arrayOfLevels[i].items[k].DefenseRatingAdd = int.Parse(data[2]);
                    arrayOfLevels[i].items[k].MagicRatingAdd = int.Parse(data[3]);
                    arrayOfLevels[i].items[k].HitPointsAdd = int.Parse(data[4]);
                    arrayOfLevels[i].items[k].ManaPointsAdd = int.Parse(data[5]);
                    arrayOfLevels[i].items[k].STRadd = int.Parse(data[6]);
                    arrayOfLevels[i].items[k].CONadd = int.Parse(data[7]);
                    arrayOfLevels[i].items[k].DEXadd = int.Parse(data[8]);
                    arrayOfLevels[i].items[k].INTadd = int.Parse(data[9]);
                    arrayOfLevels[i].items[k].WISadd = int.Parse(data[10]);
                }
                sr.ReadLine();
                int count = int.Parse(sr.ReadLine());
                for (int k = 0; k < count; k++)
                {
                    data = sr.ReadLine().Split('|');
                    arrayOfLevels[i].monsters.Add(new Monster(int.Parse(data[5]), int.Parse(data[6]), true));
                    arrayOfLevels[i].monsters[k].Name = data[0];
                    arrayOfLevels[i].monsters[k].HP = int.Parse(data[1]);
                    arrayOfLevels[i].monsters[k].Speed = int.Parse(data[2]);
                    arrayOfLevels[i].monsters[k].Attack = int.Parse(data[3]);
                    arrayOfLevels[i].monsters[k].Level = int.Parse(data[4]);
                }*/
            }
            sr.Close();
        }
    }
}
