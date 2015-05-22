using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    /// A projectile that moves in a line
    /// </summary>
    internal class LineProjectile : IUpdatable, IDrawable
    {
        public enum ProjectileStates
        {
            Dormant,
            Moving,
            Dead
        }

        private const int Duration = 1500;

        public LineProjectile(GameActor owner, Vector2 origin, Vector2 destination, double speed)
        {
            State = ProjectileStates.Dormant;
            Owner = owner;
            this.origin = origin;
            this.destination = destination;
            Speed = speed;
        }

        private Vector2 origin, destination;

        public int ElapsedTime { get; private set; }
        public ProjectileStates State { get; private set; }
        public GameActor Owner { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public double Speed { get; set; }

        public void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime >= Duration)
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

        /// <summary>
        ///     Fires the projectile
        /// </summary>
        public void Discharge()
        {
            // start at the origin
            Position = origin;

            //calculate X and Y velocity based on angle of rotation & speed
            double xDelta = destination.X - origin.X;
            double yDelta = destination.Y - origin.Y;

            //double angleInDegrees = Math.Atan2(yDelta, xDelta) * 180 / Math.PI; // degrees? radians?
            
            //Velocity = new Vector2(xSpeed, ySpeed);

            // i need to brush up on my trig god damn

            State = ProjectileStates.Moving;
            ElapsedTime = 0;
        }

        /// <summary>
        ///     Sets the projectile's state to 'Dead'
        /// </summary>
        public void Destroy()
        {
            State = ProjectileStates.Dead;
        }

        protected virtual void Move()
        {
            Position += Velocity;
        }

        protected virtual void OnCollision(GameActor collided)
        {
            if (State == ProjectileStates.Moving)
            {
                // cause damage, knockback, apply modifier?
                // this.Owner.ApplyDamage(collided, damage)
                // this.Owner.ApplyModifier(collided, stun)
                // this.Owner.Destroy(barricade)
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw projectile
        }
    }
}