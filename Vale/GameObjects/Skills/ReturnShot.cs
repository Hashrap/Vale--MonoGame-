using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Vale.GameObjects.Actors;
using Vale.GameObjects.Skills.Projectiles;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills
{
    internal class ReturnShot : QuickShot
    {
        public ReturnShot(GameplayScreen gameScreen, GameActor owner)
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
            CreateReturnProjectile(targetPosition);
            return true;
        }

        private void CreateReturnProjectile(Vector2 targetPosition)
        {
            var content = new Microsoft.Xna.Framework.Content.ContentManager(GameScreen.ScreenManager.Game.Services, "Content");
            var origin = Owner.Position;
            var rotation = Math.Atan2(targetPosition.Y - origin.Y, targetPosition.X - origin.X);
            var arrow = new ReturnProjectile(Owner.Screen, content.Load<Texture2D>("Art/bksq20x20"), Owner, Owner.Position, (float)rotation, ProjectileSpeed);
            arrow.Discharge();
            arrows.Add(arrow);
        }
    }
}