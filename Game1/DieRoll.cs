//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGen
{
    public class DieRoll
    {
        //Simulate some die rolls
        //rng object
        public MersenneTwister rng = new MersenneTwister();
        public int roll(int die, int sides)
        {
            int total = 0;
            for (int i = 0; i < die; i++)
            {
                total += rng.Next(1, sides);
            }
            return total;
        }
    }
}