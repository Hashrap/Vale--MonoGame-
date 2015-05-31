using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    ///     A projectile that moves in a line
    /// </summary>
    internal class LineProjectile : MoveableGameObject
    {
        public enum ProjectileStates
        {
            Dormant,
            Moving,
            Dead
        }

        private const int Duration = 1500;
        private readonly float spriteHeight = 20;
        private readonly float spriteWidth = 20;
        private readonly Texture2D texture;
        private readonly GameScreen gameScreen;

        public int DrawOrder { get; private set; }

        public int ElapsedTime { get; private set; }

        public Vector2 Origin { get; private set; }

        public GameActor Owner { get; private set; }

        public ProjectileStates State { get; private set; }

        public bool Visible { get; private set; }

        protected Vector2 DrawingOrigin
        {
            get { return new Vector2(spriteWidth / 2, spriteHeight / 2); }
        }

        protected Vector2 DrawingPosition
        {
            get { return Position - DrawingOrigin; }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public LineProjectile(GameScreen gameScreen, Texture2D texture, GameActor owner, Vector2 origin, float rotation, float speed)
        {
            this.gameScreen = gameScreen;
            this.texture = texture;
            State = ProjectileStates.Dormant;
            Owner = owner;
            Origin = origin;
            Position = origin;
            Rotation = rotation;
            Speed = speed;
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
            gameScreen.SpriteBatch.Draw(texture, DrawingPosition, null, Color.White, Rotation, DrawingOrigin, 1f,
                SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            switch (State)
            {
                case ProjectileStates.Dormant:
                case ProjectileStates.Dead:
                case ProjectileStates.Moving:
                    ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                    if (ElapsedTime >= Duration)
                        Destroy();
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