using Microsoft.Xna.Framework;
using System;

namespace Vale.GameObjects
{
    internal abstract class MoveableGameObject : IUpdateable
    {
        protected float rotation;

        protected float speed;

        public bool Enabled { get; private set; }

        public Vector2 Position { get; set; }

        public Vector2 PreviousPosition { get; set; }

        public float Rotation
        {
            get { return rotation; }
            protected set { rotation = value % 360; }
        }

        /// <summary>
        ///     The magnitude of this game object's velocity
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = Math.Max(0.0f, value); }
        }

        public int UpdateOrder { get; private set; }

        public Vector2 Velocity { get; set; }

        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public virtual void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        private Vector2 Move(GameTime gameTime)
        {
            Position += (Velocity * gameTime.ElapsedGameTime.Milliseconds);

            return Position;
        }
    }
}