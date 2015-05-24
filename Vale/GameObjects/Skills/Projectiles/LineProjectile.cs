using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Vale.GameObjects.Actors;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    /// A projectile that moves in a line
    /// </summary>
    internal class LineProjectile : MoveableGameObject, IDrawable
    {
        private const int Duration = 1500;
        private readonly Game1 game;
        private readonly SpriteBatch spriteBatch;
        private readonly string textureName;

        private double rotation;

        private Texture2D texture;

        public LineProjectile(Game1 game, SpriteBatch spriteBatch, string textureName)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.textureName = textureName;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public enum ProjectileStates
        {
            Dormant,
            Moving,
            Dead
        }
        public int DrawOrder { get; private set; }

        public int ElapsedTime { get; private set; }

        public Vector2 Origin { get; private set; }

        public GameActor Owner { get; private set; }

        public double Speed { get; set; }

        public ProjectileStates State { get; private set; }


        public bool Visible { get; private set; }

        public double Rotation
        {
            get {  return rotation;}
            protected set { rotation = value % 360; }
        }

        public static bool ProjectileIsDead(LineProjectile projectile)
        {
            return projectile.State == ProjectileStates.Dead;
        }

        /// <summary>
        ///     Sets the projectile's state to 'Dead'
        /// </summary>
        public void Destroy()
        {
            Velocity = Vector2.Zero;
            State = ProjectileStates.Dead;
        }

        /// <summary>
        ///     Fires the projectile
        /// </summary>
        public void Discharge()
        {
            // start at the origin
            Position = Origin;

            var xSpeed = (float)(Math.Cos(rotation) * Speed);
            var ySpeed = (float)(Math.Sin(rotation) * Speed);

            Velocity = new Vector2(xSpeed, ySpeed);

            State = ProjectileStates.Moving;
            ElapsedTime = 0;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture, Position, null, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void Initialize(GameActor owner, Vector2 origin, double rotation, double speed)
        {
            texture = game.Content.Load<Texture2D>(textureName);
            State = ProjectileStates.Dormant;
            Owner = owner;
            Origin = origin;
            Position = origin;
            Rotation = rotation;
            Speed = speed;
        }

        public override void Update(GameTime gameTime)
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
                    //Move(gameTime);
                    break;
            }

            base.Update(gameTime);
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
    }
}