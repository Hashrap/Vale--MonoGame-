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

        public CombatUnit CreateUnit(string name, GameActor.Faction alignment, Vector2 spawnPosition)
        {
            CombatUnit unit;

            if (name.StartsWith("hero_"))
            {
                unit = new Hero(game, new MouseProvider(game), new KeyboardProvider(game), alignment: alignment);
            }
            else
            {
                unit = new CombatUnit(game, alignment, spawnPosition);
            }

            unit.Position = spawnPosition;

            SetupUnit(unit, name);


            return unit;
        }

        private void SetupUnit(CombatUnit unit, string name)
        {
            unit.LoadContent(game.Content);
            
            // by now we have already parsed JSON and stored it into a dictionary.
            //then we grab all of that info and set up the unit

            var unitInfo = Resource.Instance.GetUnitInfo(name);
            unit.LoadUnitInfo(unitInfo);
        }
    }
}
