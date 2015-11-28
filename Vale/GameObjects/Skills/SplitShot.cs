using Microsoft.Xna.Framework;
using Vale.GameObjects.Actors;
using Vale.GameObjects.Skills.Hero.Archer;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills
{
    internal class SplitShot : QuickShot
    {
        private readonly double spread = .174;

        public SplitShot(GameplayScreen gameScreen, CombatUnit owner)
            : base(gameScreen, owner)
        {
        }

        /// <summary>
        ///     Fires a projectile.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected override bool DoAction(SkillArgs args, params object[] list)
        {
            var targetPosition = (Vector2)list[0]; //assumes list[0] is the target
            var rotation = CreateProjectile(targetPosition);
            CreateProjectile(rotation + spread);
            CreateProjectile(rotation - spread);
            return true;
        }
    }
}
