using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills
{
    internal class SplitShot : QuickShot
    {
        public SplitShot(GameplayScreen gameScreen, GameActor owner)
            : base(gameScreen, owner)
        {
        }

        /// <summary>
        ///     Fires a projectile.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected override bool DoAction(params object[] list)
        {
            var targetPosition = (Vector2)list[0]; //assumes list[0] is the target
            var rotation = CreateProjectile(targetPosition);
            CreateProjectile(rotation + .174);
            CreateProjectile(rotation - .174);
            return true;
        }
    }
}