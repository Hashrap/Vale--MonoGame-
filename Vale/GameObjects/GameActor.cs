using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.ScreenSystem.Screens;
using Microsoft.Xna.Framework.Content;
using Vale.GameObjects.Collision;

namespace Vale.GameObjects
{
    public abstract class GameActor : GameObject, ICollide
    {
        public enum Faction
        {
            Player,
            Hostile,
            Neutral
        }

        private AABB bounds;
        public AABB Bounds { get { return bounds; } }

        public event EventHandler BoundsChanged;
        protected void RaiseBoundsChanged()
        {
            EventHandler handler = BoundsChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }

        private float rotation;

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

        public GameplayScreen Screen { get; set; }

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

        protected GameActor(GameplayScreen screen, Faction alignment)
            : base(screen)
        {
            Screen = screen;
            Alignment = alignment;
            Visible = true;
            bounds = new AABB(DrawingPosition, spriteWidth, spriteHeight);
            Screen.Actors.Insert(this);
        }

        public Faction Alignment { set; get; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(texture, DrawingPosition, null, Color.White, Rotation, DrawingOrigin, 1f,
                    SpriteEffects.None, 0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        private Vector2 Move(GameTime gameTime)
        {
            if (Velocity == Vector2.Zero)
                return Position;

            Position += Velocity * gameTime.ElapsedGameTime.Milliseconds;
            bounds = new AABB(DrawingPosition, spriteWidth, spriteHeight);
            RaiseBoundsChanged();

            return Position;
        }

        private void OnObjectCollision() { }
        private void OnTerrainCollision() { }
    }
}