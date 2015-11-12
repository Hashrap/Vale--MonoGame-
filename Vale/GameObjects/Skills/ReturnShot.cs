using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Vale.GameObjects.Actors;
using Vale.GameObjects.Skills.Hero.Archer;
using Vale.GameObjects.Skills.Projectiles;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills
{
    internal class ReturnShot : QuickShot
    {
        public ReturnShot(GameplayScreen gameScreen, CombatUnit owner)
            : base(gameScreen, owner)
        {
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("Art/return20x20");
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

        protected void CreateReturnProjectile(Vector2 targetPosition)
        {
            var origin = Owner.Position;
            var rotation = Math.Atan2(targetPosition.Y - origin.Y, targetPosition.X - origin.X);
            var arrow = new ReturnProjectile(Owner.Game, texture, Owner, Owner.Position, (float)rotation, ProjectileSpeed);
            arrow.LoadContent();
            arrow.Discharge();
            arrows.Add(arrow);
            Game.AddObject(arrow);
        }
    }
}
