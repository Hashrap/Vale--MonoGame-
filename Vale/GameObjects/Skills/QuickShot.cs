using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    ///     Fires an arrow in the direction of the cursor.
    /// </summary>
    internal class QuickShot : Skill
    {
        public readonly float ProjectileSpeed = .75f;
        // this should be read in from a parsed file
        protected readonly List<LineProjectile> arrows;
        protected Texture2D texture;

        public QuickShot(GameplayScreen gameScreen, GameActor owner)
            : base(gameScreen, owner)
        {
            arrows = new List<LineProjectile>();
        }

        public override void LoadContent(ContentManager content)
        {
            texture = Game.Content.Load<Texture2D>("Art/quickshot10x20");
        }

        public override void Update(GameTime gameTime)
        {
            List<LineProjectile> toRemove = arrows.FindAll(LineProjectile.ProjectileIsDead);

            foreach (var arrow in toRemove) {
                arrows.Remove(arrow);
                Game.RemoveObject(arrow);
            }

            base.Update(gameTime);
        }

        /// <summary>
        ///     Fires a projectile.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected override bool DoAction(params object[] list)
        {
            var targetPosition = (Vector2)list[0]; //assumes list[0] is the target
            CreateProjectile(targetPosition);
            return true;
        }

        protected double CreateProjectile(Vector2 targetPosition)
        {
            var origin = Owner.Position;
            var rotation = Math.Atan2(targetPosition.Y - origin.Y, targetPosition.X - origin.X);
            CreateProjectile(rotation);
            return rotation;
        }

        protected void CreateProjectile(double rotation)
        {
            var arrow = new LineProjectile(Owner.Game, texture, Owner, Owner.Position, (float)rotation, ProjectileSpeed);
            arrow.Discharge();
            arrows.Add(arrow);
            Game.AddObject(arrow);
        }
    }
}