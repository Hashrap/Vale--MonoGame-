﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects
{
    class UnitFactory
    {
        private GameplayScreen game;

        public UnitFactory(GameplayScreen game)
        {
            this.game = game;
        }

        public CombatUnit CreateUnit(string name, GameActor.Faction alignment)
        {
            CombatUnit unit;

            if (name.StartsWith("hero_"))
            {
                unit = new Hero(game, new MouseProvider(game), new KeyboardProvider(game), alignment: alignment);
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
