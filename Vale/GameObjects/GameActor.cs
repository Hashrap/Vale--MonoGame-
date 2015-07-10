using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Vale.ScreenSystem.Screens;
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

        protected AABB bounds;
        public AABB Bounds { get { return bounds; } }
        public Vector2 Position { get { return Bounds.Center; } }

        protected AABB previousBounds;
        public AABB PreviousBounds { get { return previousBounds; } }
        public Vector2 PreviousPosition { get { return PreviousBounds.Center; } }

        public event EventHandler BoundsChanged;

        private float rotation;

        protected float speed;

        protected Texture2D texture;

        private float spriteWidth = 20, spriteHeight = 20;

        public bool Enabled { get; private set; }

        public float Rotation
        {
            get { return rotation; }
            protected set { rotation = value % 360; }
        }

        public GameplayScreen Screen { get; protected set; }

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

        protected GameActor(GameplayScreen screen, Faction alignment, Vector2 pos)
            : base(screen)
        {
            Screen = screen;
            Alignment = alignment;
            bounds = new AABB(pos, spriteWidth, spriteHeight);
            previousBounds = bounds;
            Visible = true;
            Game.Actors.Insert(this);
        }

        public Faction Alignment { set; get; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(texture, Position, null, Color.White, Rotation, new Vector2(Bounds.HalfWidth, Bounds.HalfHeight), 1f,
                    SpriteEffects.None, 0f);
            }
        }

        public void DebugDraw(Texture2D texture, SpriteBatch spriteBatch)
        {
            //Bounds
            spriteBatch.Draw(texture, new Rectangle((int)Bounds.Left, (int)Bounds.Top, 1, 1), Color.Yellow);
            spriteBatch.Draw(texture, new Rectangle((int)Bounds.Right, (int)Bounds.Bottom, 1, 1), Color.Purple);
            spriteBatch.Draw(texture, new Rectangle((int)Bounds.Right, (int)Bounds.Top, 1, 1), Color.Red);
            spriteBatch.Draw(texture, new Rectangle((int)Bounds.Left, (int)Bounds.Bottom, 1, 1), Color.Green);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        private Vector2 Move(GameTime gameTime)
        {
            previousBounds = Bounds;
            if (Velocity == Vector2.Zero)
                return Position;

            Vector2 distance = Velocity * gameTime.ElapsedGameTime.Milliseconds;
            bounds = new AABB(Bounds.Origin + distance, spriteWidth, spriteHeight);
            if (!Screen.Map.Query(Bounds))
                OnTerrainCollision();

            if (Bounds != PreviousBounds)
                RaiseBoundsChanged();

            return Position;
        }

        protected void RaiseBoundsChanged()
        {
            EventHandler handler = BoundsChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }

        protected virtual void OnObjectCollision() { }
        protected virtual void OnTerrainCollision() 
        {
            if (!Screen.Map.Query(new AABB(new Vector2(Bounds.X, PreviousBounds.Y), spriteWidth, spriteHeight)))
                bounds = new AABB(new Vector2(PreviousBounds.X, Bounds.Y), spriteWidth, spriteHeight);
            if (!Screen.Map.Query(new AABB(new Vector2(PreviousBounds.X, Bounds.Y), spriteWidth, spriteHeight)))
                bounds = new AABB(new Vector2(Bounds.X, PreviousBounds.Y), spriteWidth, spriteHeight);
        }
    }
}