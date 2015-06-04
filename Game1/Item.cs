//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DungeonGen
{
    public class Item
    {
        //attributes
        public enum Type { Weapon = 0, Armor = 1, Potion = 2, Scroll = 3, Tool = 4 }
        private Type type;
        private string category;
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string suffix;
        private bool hasSuffix;
        private string prefix;
        private bool hasPrefix;
        private string location;
        private int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        private int attackRatingAdd;
        public int AttackRatingAdd
        {
            get { return attackRatingAdd; }
            set { attackRatingAdd = value; }
        }
        private int defenseRatingAdd;
        public int DefenseRatingAdd
        {
            get { return defenseRatingAdd; }
            set { defenseRatingAdd = value; }
        }
        private int magicRatingAdd;
        public int MagicRatingAdd
        {
            get { return magicRatingAdd; }
            set { magicRatingAdd = value; }
        }
        private int hitPointsAdd;
        public int HitPointsAdd
        {
            get { return hitPointsAdd; }
            set { hitPointsAdd = value; }
        }
        private int manaPointsAdd;
        public int ManaPointsAdd
        {
            get { return manaPointsAdd; }
            set { manaPointsAdd = value; }
        }
        private int strAdd;
        public int STRadd
        {
            get { return strAdd; }
            set { strAdd = value; }
        }
        private int conAdd;
        public int CONadd
        {
            get { return conAdd; }
            set { conAdd = value; }
        }
        private int dexAdd;
        public int DEXadd
        {
            get { return dexAdd; }
            set { dexAdd = value; }
        }
        private int intAdd;
        public int INTadd
        {
            get { return intAdd; }
            set { intAdd = value; }
        }
        private int wisAdd;
        public int WISadd
        {
            get { return wisAdd; }
            set { wisAdd = value; }
        }
        //Constructor
        public Item(int x, int y, bool load)
        {
            MersenneTwister rng = new MersenneTwister();
            attackRatingAdd = 0;
            defenseRatingAdd = 0;
            magicRatingAdd = 0;
            strAdd = 0;
            conAdd = 0;
            dexAdd = 0;
            intAdd = 0;
            wisAdd = 0;
            prefix = "";
            suffix = "";
            name = "";
            this.x = x;
            this.y = y;
            location = this.x + " " + this.y + " ";
            /*This boolean tells the constructor whether to create
             * the item from scratch or whether it'll be loaded out
             * of a txt file*/
            if (load == false)
            {
                switch (rng.Next(3, 5)) //I've only implemented armor and weapons
                {
                    case 0:
                        type = Type.Tool;
                        category = "tool";
                        break;
                    case 1:
                        type = Type.Scroll;
                        category = "scroll";
                        break;
                    case 2:
                        type = Type.Potion;
                        category = "potion";
                        break;
                    case 3:
                        type = Type.Armor;
                        category = "armor";
                        determinePreSuf();
                        determineArms();
                        break;
                    case 4:
                        type = Type.Weapon;
                        category = "weapon";
                        determinePreSuf();
                        determineArms();
                        break;
                }
            }
        }

        public void determinePreSuf()
        {
            MersenneTwister rng = new MersenneTwister();
            int wait = 0;
            while (wait < 10000000)
                wait++;
            if (rng.Next(100) > 40)
            {
                hasPrefix = true;
                string[] split = new string[12];
                bool good = false;
                while (good == false)
                {
                    StreamReader sr = new StreamReader("Content/txt/prefix.txt");
                    //Randomly pick a prefix and load it in
                    int r = rng.Next(1, 70);
                    for (int i = 0; i < r; i++)
                        sr.ReadLine();
                    split = sr.ReadLine().Split('|');
                    if (rng.NextDoublePositive() < double.Parse(split[11]))
                        good = true;
                    sr.Close();
                }
                prefix = split[0];
                attackRatingAdd += int.Parse(split[1]);
                defenseRatingAdd += int.Parse(split[2]);
                magicRatingAdd += int.Parse(split[3]);
                hitPointsAdd += int.Parse(split[4]);
                manaPointsAdd += int.Parse(split[5]);
                strAdd += int.Parse(split[6]);
                conAdd += int.Parse(split[7]);
                dexAdd += int.Parse(split[8]);
                intAdd += int.Parse(split[9]);
                wisAdd += int.Parse(split[10]);
            }
            if (rng.Next(100) > 40)
            {
                hasSuffix = true;
                string[] split = new string[12];
                bool good = false;
                while (good == false)
                {
                    StreamReader sr = new StreamReader("Content/txt/suffix.txt");
                    //Randomly pick a suffix and load it in
                    int r = rng.Next(1, 75);
                    for (int i = 0; i < r; i++)
                        sr.ReadLine();
                    split = sr.ReadLine().Split('|');
                    if (rng.NextDoublePositive() < double.Parse(split[11]))
                        good = true;
                    sr.Close();
                }
                suffix = split[0];
                attackRatingAdd += int.Parse(split[1]);
                defenseRatingAdd += int.Parse(split[2]);
                magicRatingAdd += int.Parse(split[3]);
                hitPointsAdd += int.Parse(split[4]);
                manaPointsAdd += int.Parse(split[5]);
                strAdd += int.Parse(split[6]);
                conAdd += int.Parse(split[7]);
                dexAdd += int.Parse(split[8]);
                intAdd += int.Parse(split[9]);
                wisAdd += int.Parse(split[10]);
            }
        }
        public void determineArms()
        {
            MersenneTwister rng = new MersenneTwister();
            string[] split = new string[12];
            bool good = false;
            while (good == false)
            {
                StreamReader sr = new StreamReader("Content/txt/" + category + ".txt");
                //Randomly pick armor type and load it in
                int r = rng.Next(1, 28);
                for (int i = 0; i < r; i++)
                    sr.ReadLine();
                string data = sr.ReadLine();
                split = data.Split('|');
                if (rng.NextDoublePositive() < double.Parse(split[11]))
                    good = true;
                sr.Close();
            }
            //We can probably refactor out these if statements.  They're
            //legacies from back when the Item class was constructed differently
            if (hasSuffix == true && hasPrefix == true)
                name = prefix + " " + split[0] + suffix;
            else if (hasPrefix == true && hasSuffix == false)
                name = prefix + " " + split[0];
            else if (hasSuffix == true && hasPrefix == false)
                name = split[0] + suffix;
            else
                name = split[0];
            attackRatingAdd += int.Parse(split[1]);
            defenseRatingAdd += int.Parse(split[2]);
            magicRatingAdd += int.Parse(split[3]);
            hitPointsAdd += int.Parse(split[4]);
            manaPointsAdd += int.Parse(split[5]);
            strAdd += int.Parse(split[6]);
            conAdd += int.Parse(split[7]);
            dexAdd += int.Parse(split[8]);
            intAdd += int.Parse(split[9]);
            wisAdd += int.Parse(split[10]);
        }
        public string ToString(bool save)
        {
            //The passed boolean indicates whether to use console syntax or savegame syntax
            if (save == false)
                return name + "\nAR = " + attackRatingAdd + "\nDR = " + defenseRatingAdd + "\nMR = " + magicRatingAdd + "\nHP = " + hitPointsAdd
                    + "\nMP = " + manaPointsAdd + "\nSTR = " + strAdd + "\nCON = " + conAdd + "\nDEX = " + dexAdd
                    + "\nINT = " + intAdd + "\nWIS = " + wisAdd + "\nX = " + x + "\nY = " + y;
            else
                return name + "|" + attackRatingAdd + "|" + defenseRatingAdd + "|" + magicRatingAdd + "|" + hitPointsAdd
                    + "|" + manaPointsAdd + "|" + strAdd + "|" + conAdd + "|" + dexAdd
                    + "|" + intAdd + "|" + wisAdd + "|" + x + "|" + y;
        }
        public void testItems()
        {
            while (true)
            {
                attackRatingAdd = 0;
                defenseRatingAdd = 0;
                magicRatingAdd = 0;
                hitPointsAdd = 0;
                manaPointsAdd = 0;
                strAdd = 0;
                conAdd = 0;
                dexAdd = 0;
                intAdd = 0;
                wisAdd = 0;
                determineArms();
                Console.WriteLine(ToString(false));
            }
        }
    }
}