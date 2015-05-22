using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vale.Skills
{
    class QuickShot : Skill
    {
        /// <summary>
        /// Fires a projectile.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected override bool DoAction(params object[] list)
        {
            Vector2 target = (Vector2)list[0];
            Vector2 origin = Owner.Position;

            double xDelta = target.X - origin.X;
            double yDelta = target.Y - origin.Y;

            double angleInDegrees = Math.Atan2(yDelta, xDelta) * 180 / Math.PI; // degrees? radians?

            Vector2 velocity = new Vector2(10, 10);

            LineProjectile arrow = new LineProjectile(this.Owner, origin, velocity);
            arrow.Discharge();

            return true;
        }

        public QuickShot(GameActor owner)
            : base(owner)
        {

        }
    }
}
