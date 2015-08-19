using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills.Hero.Archer
{
    /// <summary>
    ///     Fires an arrow in the direction of the cursor.
    /// </summary>
    internal class QuickShot : Skill
    {
        public struct ChargeInfo
        {
            public bool PreviousChargeState, CurrentChargeState;
            public double ElapsedChargeTime, MaxChargeTime;

            public ChargeInfo(double maxChargeTime)
            {
                PreviousChargeState = false;
                CurrentChargeState = false;
                ElapsedChargeTime = 0;
                MaxChargeTime = maxChargeTime;
            }
        }

        public readonly float ProjectileSpeed = .75f;
        // this should be read in from a parsed file
        protected readonly List<LineProjectile> arrows;
        protected Texture2D texture;

        private ChargeInfo chargeInfo;

        public QuickShot(GameplayScreen gameScreen, CombatUnit owner)
            : base(gameScreen, owner)
        {
            arrows = new List<LineProjectile>();
            chargeInfo = new ChargeInfo();
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("Art/quickshot10x20");
        }

        public override void Update(GameTime gameTime)
        {
            List<LineProjectile> toRemove = arrows.FindAll(LineProjectile.ProjectileIsDead);

            chargeInfo.PreviousChargeState = chargeInfo.CurrentChargeState;

            foreach (var arrow in toRemove)
            {
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
            if (Status == SkillState.Available)
            {
                // begin charging


                var targetPosition = (Vector2)list[0]; //assumes list[0] is the target
                CreateProjectile(targetPosition);
            }
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
            arrow.LoadContent();
            arrow.Discharge();
            arrows.Add(arrow);
            Game.AddObject(arrow);
        }
    }
}