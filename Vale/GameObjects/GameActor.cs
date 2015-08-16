using System;
using System.Collections.Generic;
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

        public Faction Alignment { set; get; }

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
        protected float SpriteHeight { get { return texture.Height; } }
        protected float SpriteWidth { get { return texture.Width; } }

        public bool Enabled { get; private set; }

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

        public Vector2 Velocity { get; set; }

        public bool Visible { get; set; }

        protected GameActor(GameplayScreen game, Faction alignment, Vector2 pos)
            : base(game)
        {
            Alignment = alignment;
            bounds = new AABB(pos, 0, 0);
            previousBounds = bounds;
            Visible = true;
        }

        public override void LoadContent()
        {
            bounds = new AABB(Bounds.Origin, SpriteWidth, SpriteHeight);
            Game.Actors.Insert(this);
        }

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
            OnObjectCollision(Game.Actors.Query(Bounds));
        }

        private Vector2 Move(GameTime gameTime)
        {
            previousBounds = Bounds;
            if (Velocity == Vector2.Zero)
                return Position;

            Vector2 distance = Velocity * gameTime.ElapsedGameTime.Milliseconds;
            bounds = new AABB(Bounds.Origin + distance, SpriteWidth, SpriteHeight);
            if (!Game.Map.Query(Bounds))
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

        protected virtual void OnObjectCollision(List<GameActor> collisions)
        {
            // Nothing~
        }
        protected virtual void OnTerrainCollision() 
        {
            Point nw = Game.Map.GetTileCoordinates(Bounds.Origin);
            Point se = Game.Map.GetTileCoordinates(Bounds.Opposite);
            Point pnw = Game.Map.GetTileCoordinates(PreviousBounds.Origin);
            Point pse = Game.Map.GetTileCoordinates(PreviousBounds.Opposite);

            // Check horizontal
            if (!Game.Map.Query(new AABB(new Vector2(Bounds.X, PreviousBounds.Y), SpriteWidth, SpriteHeight)))
                // Wall located East
                if(se.X - pse.X > 0)
                    bounds = new AABB(new Vector2(se.X * Game.Map.TileWidth - (SpriteWidth+1), Bounds.Y), SpriteWidth, SpriteHeight);
                // Wall located West
                else if(nw.X - pnw.X < 0)
                    bounds = new AABB(new Vector2(pnw.X * Game.Map.TileWidth + 1, Bounds.Y), SpriteWidth, SpriteHeight);
            // Check vertical
            if (!Game.Map.Query(new AABB(new Vector2(PreviousBounds.X, Bounds.Y), SpriteWidth, SpriteHeight)))
                // Wall located South
                if (se.Y - pse.Y > 0)
                    bounds = new AABB(new Vector2(Bounds.X, se.Y * Game.Map.TileHeight - (SpriteHeight+1)), SpriteWidth, SpriteHeight);
                // Wall located North
                else if (nw.Y - pnw.Y < 0)
                    bounds = new AABB(new Vector2(Bounds.X, pnw.Y * Game.Map.TileHeight + 1), SpriteWidth, SpriteHeight);
        }
    }
}