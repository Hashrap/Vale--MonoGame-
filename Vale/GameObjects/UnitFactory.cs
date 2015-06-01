using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem;

namespace Vale.GameObjects
{
    class UnitFactory
    {
        private GameScreen game;

        public UnitFactory(GameScreen game)
        {
            this.game = game;
        }

        public CombatUnit CreateUnit(string name, GameActor.Faction alignment)
        {
            CombatUnit unit;

            if (name.StartsWith("hero_"))
            {
                unit = new Hero(game, alignment);
            }
            else
            {
                unit = new CombatUnit(game, alignment);
            }




            return unit;
        }

        private void SetupUnit(CombatUnit unit, string name)
        {
            // by now we have already parsed JSON and stored it into a dictionary.
            //then we grab all of that info and set up the unit

            // sprite, sprite size
            // speed, health
            // skills
            // drops? exp gain? gold gain?
        }
    }
}
