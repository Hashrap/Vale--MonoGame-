using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Vale.GameObjects.Actors;
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
        private static Texture2D texture;

        public QuickShot(GameplayScreen gameScreen, GameActor owner)
            : base(gameScreen, owner)
        {
            arrows = new List<LineProjectile>();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (var arrow in arrows.Where(arrow => arrow.State == LineProjectile.ProjectileStates.Moving))
                arrow.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var arrow in arrows)
                arrow.Update(gameTime);

            arrows.RemoveAll(LineProjectile.ProjectileIsDead);
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
            if (texture == null)
            {
                var content = new Microsoft.Xna.Framework.Content.ContentManager(GameScreen.ScreenManager.Game.Services, "Content");
                texture = content.Load<Texture2D>("Art/arrow20x20");
            }
            var arrow = new LineProjectile(Owner.Screen, texture, Owner, Owner.Position, (float)rotation, ProjectileSpeed);
            arrow.Discharge();
            arrows.Add(arrow);
        }
    }
}