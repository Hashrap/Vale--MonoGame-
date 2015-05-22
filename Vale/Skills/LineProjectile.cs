using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vale.Skills
{
    class LineProjectile : IUpdatable
    {
        public enum ProjectileStates { Dormant, Moving, Dead }

        private const int Duration = 1500;
        private int elapsedTime;

        private ProjectileStates State { set; get; }
        public GameActor Owner { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }

        public LineProjectile(GameActor owner, Vector2 position, Vector2 velocity)
        {
            State = ProjectileStates.Dormant;
            Owner = owner;
            this.Position = position;
            this.Velocity = velocity;
        }

        /// <summary>
        /// Fires the projectile
        /// </summary>
        public void Discharge()
        {
            State = ProjectileStates.Moving;
        }

        /// <summary>
        /// Sets the projectile's state to 'Dead'
        /// </summary>
        public void Destroy()
        {
            State = ProjectileStates.Dead;
        }


        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime >= Duration)
                Destroy();

            switch (State)
            {
                case ProjectileStates.Dormant:
                case ProjectileStates.Dead:
                    break;
                case ProjectileStates.Moving:
                    Move();
                    break;

            }
        }

        protected virtual void Move()
        {
            Position += Velocity;
        }

        protected virtual void OnCollision(GameActor collided)
        {
            // cause damage, knockback, apply modifier, lifesteal?
            // this.Owner.ApplyDamage(collided, damage)
            // this.Owner.ApplyModifier(collided, stun)
            // this.Owner.Destroy(barricade)
        }
    }
}
