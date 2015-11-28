using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Vale.GameObjects.Actors;
using Vale.GameObjects.Collision;
using Vale.ScreenSystem.Screens;

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

        private float speed;

        /// <summary>
        ///     The magnitude of this game object's velocity
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = Math.Max(0.0f, value); }
        }

        private const int Duration = 1500;
        private readonly GameplayScreen gameScreen;

        public int ElapsedTime { get; private set; }

        public CombatUnit Owner { get; private set; }

        public ProjectileStates State { get; private set; }




        public LineProjectile(GameplayScreen gameScreen, Texture2D texture, CombatUnit owner, Vector2 origin, Vector2 size, float rotation, float speed)
            : base(gameScreen, origin, size)
        {
            this.gameScreen = gameScreen;
            this.Texture = texture;
            bounds = new AABB(new Vector2(origin.X-(SpriteWidth/2),origin.Y-(SpriteHeight/2)), SpriteWidth, SpriteHeight);
            State = ProjectileStates.Dormant;
            Owner = owner;
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
            Game.Actors.Remove(this);
        }

        /// <summary>
        ///     Fires the projectile
        /// </summary>
        public void Discharge()
        {
            Velocity = new Vector2((float)(Math.Cos(Rotation) * Speed), (float)(Math.Sin(Rotation) * Speed));

            State = ProjectileStates.Moving;
            ElapsedTime = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (State == LineProjectile.ProjectileStates.Moving)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, new Vector2(Bounds.HalfWidth, Bounds.HalfHeight), 1f,
                    SpriteEffects.None, 0f);
            }
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

        protected virtual void OnCollision(GameObject collided)
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