using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.ScreenSystem;

namespace Vale.GameObjects
{
    internal abstract class GameActor : IUpdate, IDraw
    {
        public enum Faction
        {
            Player,
            Hostile,
            Neutral
        }

        protected float rotation;

        protected float speed;

        protected Texture2D texture;

        private float spriteWidth = 20, spriteHeight = 20;

        public bool Enabled { get; private set; }

        public Vector2 Position { get; set; }

        public Vector2 PreviousPosition { get; set; }

        public float Rotation
        {
            get { return rotation; }
            protected set { rotation = value % 360; }
        }

        public GameScreen Screen { get; set; }

        /// <summary>
        ///     The magnitude of this game object's velocity
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = Math.Max(0.0f, value); }
        }

        public Vector2 Velocity { get; set; }

        public bool Visible { get; set; }

        protected Vector2 DrawingOrigin
        {
            get { return new Vector2(spriteWidth / 2, spriteHeight / 2); }
        }

        protected Vector2 DrawingPosition
        {
            get { return Position - DrawingOrigin; }
        }

        protected GameActor(GameScreen screen, Faction alignment)
        {
            Screen = screen;
            Alignment = alignment;
            Visible = true;
        }

        public Faction Alignment { set; get; }

        public virtual void Draw(GameTime gameTime)
        {
            if (Visible)
                Screen.SpriteBatch.Draw(texture, DrawingPosition, null, Color.White, Rotation, DrawingOrigin, 1f,
                    SpriteEffects.None, 0f);
        }

        public virtual void LoadContent()
        {
        }

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