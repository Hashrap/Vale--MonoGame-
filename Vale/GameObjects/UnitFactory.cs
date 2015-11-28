using Microsoft.Xna.Framework;
using Vale.GameObjects.Actors;
using Vale.Parsing;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects
{
    public class UnitFactory
    {
        private GameplayScreen game;

        public UnitFactory(GameplayScreen game)
        {
            this.game = game;
        }

        public CombatUnit CreateUnit(string name, CombatUnit.Faction alignment, Vector2 spawnPosition)
        {
            CombatUnit unit;

            if (name.StartsWith("hero_"))
            {
                unit = new Hero(game, new MouseProvider(game), new KeyboardProvider(game), spawnPosition, new Vector2(10,10), alignment: alignment);
            }
            else
            {
                unit = new CombatUnit(game, alignment, spawnPosition, new Vector2(10,10));
            }

            SetupUnit(unit, name);

            return unit;
        }

        private void SetupUnit(CombatUnit unit, string name)
        {
            unit.LoadContent();
            
            // by now we have already parsed JSON and stored it into a dictionary.
            //then we grab all of that info and set up the unit

            var unitInfo = Resource.Instance.GetUnitInfo(name);
            unit.LoadUnitInfo(unitInfo);
        }
    }
}
