//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DungeonGen
{
    //This class generates dungeons using one of several algorithms, depending on the method called
    public class Generator
    {
        //Attributes and objects
        public enum DungeonType { Cave, Dungeon, Forest };
        public DungeonType type;
        public Map[] arrayOfMaps;
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

        public void Cave(int levels, string protocol, int wall, int size_x, int size_y)
        {
            arrayOfMaps = new CaveLevel[levels];
            for (int i = 0; i < levels; i++)
            {
                CaveLevel cl;
                bool good = false;
                int trashed = 0;
                while (good != true)
                {
                    cl = new CaveLevel(size_y, size_x);
                    cl.setWall();
                    cl.randomFill(wall);

                    foreach (char ch in protocol)
                    {
                        if (ch == '1')
                            cl.ageDungeon(1);
                        if (ch == '2')
                            cl.ageDungeon(2);
                    }

                    arrayOfMaps[i] = cl;

                    /* These check the validity of the map, trashes "bad" maps until it gets a good one
                     * Will attempt to repair mildly disjointed rooms.*/
                    arrayOfMaps[i].setInvalid();
                    int[] fTile = arrayOfMaps[i].findFloor();
                    arrayOfMaps[i].iterativeFloodSearch(fTile[0], fTile[1]);
                    /*if (arrayOfMaps[i].isEverythingReachable() == false && arrayOfMaps[i].BadCount > (size_x * size_y)/4)
                    {
                        good = false; //Trashes the map
                        trashed++;
                        Console.WriteLine(trashed);
                    }
                    else if (arrayOfMaps[i].isEverythingReachable() == false && arrayOfMaps[i].BadCount <= (size_x * size_y)/4)
                    {
                        arrayOfMaps[i].fill(); //Repairs the map
                        good = true;
                    }
                    else*/
                        good = true; //Map is fine as is
                    arrayOfMaps[i].placeObjects();
                    arrayOfMaps[i].setInvalid();
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
            arrayOfMaps = new DungeonLevel[levels];
            for (int i = 0; i < levels; i++)
            {
                DungeonLevel dl = new DungeonLevel(size_y, size_x);
                dl.dungeonGen(iterations, pos_min, pos_max, min_room);
                arrayOfMaps[i] = dl;
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
            sr.WriteLine(arrayOfMaps.Length);
            for (int i = 0; i < arrayOfMaps.Length; i++)
            {
                sr.WriteLine(i + 1);
                sr.WriteLine(arrayOfMaps[i].Size_Y + " " + arrayOfMaps[i].Size_X);
                for (int k = 0; k < arrayOfMaps[i].board.GetLength(0); k++)
                {
                    for (int j = 0; j < arrayOfMaps[i].board.GetLength(1); j++)
                    {
                        sr.Write((int)arrayOfMaps[i].board[k, j] + "|");
                    }
                    sr.WriteLine();
                }
                sr.WriteLine("[Items]");
                sr.WriteLine(arrayOfMaps[i].items.Length);
                foreach (Item item in arrayOfMaps[i].items)
                    sr.WriteLine(item.ToString(true));
                sr.WriteLine("[Monsters]");
                sr.WriteLine(arrayOfMaps[i].monsters.Count());
                foreach (Monster monster in arrayOfMaps[i].monsters)
                    sr.WriteLine(monster.ToString(true));
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
            arrayOfMaps = new CaveLevel[int.Parse(sr.ReadLine())]; //load # of levels
            for (int i = 0; i < arrayOfMaps.Length; i++)
            {
                sr.ReadLine(); //skip a line
                data = sr.ReadLine().Split(' '); //Load the dimensions into the array
                //Create a new CaveLevel of the saved dimensions
                CaveLevel cl = new CaveLevel(int.Parse(data[0]), int.Parse(data[1]));
                //and copy it into the array
                arrayOfMaps[i] = cl;
                for (int k = 0; k < arrayOfMaps[i].board.GetLength(0); k++)
                {
                    data = sr.ReadLine().Split('|');
                    for (int j = 0; j < data.Length - 1; j++)
                    {
                        //Copies the actual map from the file
                        arrayOfMaps[i].board[k, j] = (DungeonGen.Map.Tile)int.Parse(data[j]);
                    }
                }
                sr.ReadLine();//Skip a line
                //Make a new array of items
                arrayOfMaps[i].items = new Item[int.Parse(sr.ReadLine())];
                for (int k = 0; k < arrayOfMaps[i].items.Length; k++)
                {
                    data = sr.ReadLine().Split('|');//AND MY AXE
                    //Make a new item at the saved location
                    Item item = new Item(int.Parse(data[12]), int.Parse(data[11]), true);
                    //and then copy it into the array and set attributes from the file
                    arrayOfMaps[i].items[k] = item;
                    arrayOfMaps[i].items[k].Name = data[0];
                    arrayOfMaps[i].items[k].AttackRatingAdd = int.Parse(data[1]);
                    arrayOfMaps[i].items[k].DefenseRatingAdd = int.Parse(data[2]);
                    arrayOfMaps[i].items[k].MagicRatingAdd = int.Parse(data[3]);
                    arrayOfMaps[i].items[k].HitPointsAdd = int.Parse(data[4]);
                    arrayOfMaps[i].items[k].ManaPointsAdd = int.Parse(data[5]);
                    arrayOfMaps[i].items[k].STRadd = int.Parse(data[6]);
                    arrayOfMaps[i].items[k].CONadd = int.Parse(data[7]);
                    arrayOfMaps[i].items[k].DEXadd = int.Parse(data[8]);
                    arrayOfMaps[i].items[k].INTadd = int.Parse(data[9]);
                    arrayOfMaps[i].items[k].WISadd = int.Parse(data[10]);
                }
                sr.ReadLine();
                int count = int.Parse(sr.ReadLine());
                for (int k = 0; k < count; k++)
                {
                    data = sr.ReadLine().Split('|');
                    arrayOfMaps[i].monsters.Add(new Monster(int.Parse(data[5]), int.Parse(data[6]), true));
                    arrayOfMaps[i].monsters[k].Name = data[0];
                    arrayOfMaps[i].monsters[k].HP = int.Parse(data[1]);
                    arrayOfMaps[i].monsters[k].Speed = int.Parse(data[2]);
                    arrayOfMaps[i].monsters[k].Attack = int.Parse(data[3]);
                    arrayOfMaps[i].monsters[k].Level = int.Parse(data[4]);
                }
            }
            sr.Close();
        }

        public int[,] exportVale(int index)
        {
            return arrayOfMaps[index].exportMap();
        }
    }
}