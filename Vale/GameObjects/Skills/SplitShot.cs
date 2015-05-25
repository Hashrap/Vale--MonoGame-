using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;

namespace Vale.GameObjects.Skills
{
    internal class SplitShot : QuickShot
    {
        public SplitShot(Game1 game, SpriteBatch spriteBatch, GameActor owner)
            : base(game, spriteBatch, owner)
        {
        }

        /// <summary>
        ///     Fires a projectile.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected override bool DoAction(params object[] list)
        {
            var targetPosition = (Vector2) list[0]; //assumes list[0] is the target
            var rotation = CreateProjectile(targetPosition);
            CreateProjectile(rotation + .174);
            CreateProjectile(rotation - .174);
            return true;
        }
    }
}