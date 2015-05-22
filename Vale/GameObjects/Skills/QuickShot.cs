using Microsoft.Xna.Framework;
using Vale.GameObjects.Actors;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    /// Fires an arrow in the direction of the cursor.
    /// </summary>
    class QuickShot : Skill
    {
        public const double ProjectileSpeed = 10;

        /// <summary>
        /// Fires a projectile.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected override bool DoAction(params object[] list)
        {
            Vector2 target = (Vector2)list[0]; //assumes list[0] is the target
            Vector2 origin = Owner.Position;

            var arrow = new LineProjectile(this.Owner, origin, target, ProjectileSpeed);
            arrow.Discharge();

            return true;
        }

        public QuickShot(GameActor owner)
            : base(owner)
        {
            // probably set some values here? parse a file? maybe parse BEFORE and pull that info now?
        }
    }
}
