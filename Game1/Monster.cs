using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DungeonGen
{
    public class Monster
    {
        MersenneTwister rng = new MersenneTwister();
        private int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        private int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int speed;
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        private int hp;
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }
        private int attack;
        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        private int level;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Monster(int x, int y, bool load)
        {
            this.y = y;
            this.x = x;
            speed = 200;
            hp = 50;
            attack = 20;
            name = "Missingno.";
            if (load == false)
                determineMonster();
        }

        private void determineMonster()
        {
            int wait = 0;
            while (wait < 10000000)
                wait++;
            string[] split;
            StreamReader sr = new StreamReader("Content\\txt\\monster.txt");
            int r = rng.Next(1, 38);
            for (int i = 0; i < r; i++)
            {
                sr.ReadLine();
            }
            split = sr.ReadLine().Split('|');
            name = split[0];
            hp = int.Parse(split[1]);
            speed = int.Parse(split[2]);
            attack = int.Parse(split[3]);
            level = int.Parse(split[4]);
        }
        public string ToString(bool load)
        {
            if (load == false)
            {
                return "Name = " + name + "\nHP = " + hp + "\nSpeed = " + speed + "\nAttack = "
                    + attack + "\nLevel = " + level + "\nX = "+x+"\nY = "+y;
            }
            else 
            {
                return name + "|" + hp + "|" + speed + "|"
                    + attack + "|" + level + "|"+x+"|"+y;
            }
        }
    }
}
