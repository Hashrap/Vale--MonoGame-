﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;
using Vale.GameObjects.Skills.Projectiles;

namespace Vale.GameObjects.Skills
{
    class ReturnShot : QuickShot
    {
        public ReturnShot(Game1 game, SpriteBatch spriteBatch, GameActor owner)
            : base(game, spriteBatch, owner)
        {
        }

        /// <summary>
        /// Fires a projectile.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected override bool DoAction(params object[] list)
        {
            var targetPosition = (Vector2)list[0]; //assumes list[0] is the target
            CreateReturnProjectile(targetPosition);
            return true;
        }

        private void CreateReturnProjectile(Vector2 targetPosition)
        {
            var origin = Owner.Position;
            var rotation = Math.Atan2(targetPosition.Y - origin.Y, targetPosition.X - origin.X);
            var arrow = new ReturnProjectile(Owner.Game, Owner.SprtBatch, "Art\\bksq20x20");
            arrow.Initialize(Owner, Owner.Position, rotation, ProjectileSpeed);
            arrow.Discharge();
            arrows.Add(arrow);
        }
    }

}
