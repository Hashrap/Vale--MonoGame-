using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Collision;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects
{
    public abstract class GameObject : IDraw, IUpdate, ICollide
    {
        public GameplayScreen Game { get; set; }

        public GameObject(GameplayScreen game, Vector2 position, Vector2 size)
        {
            Visible = true;
            this.Game = game;
            bounds = new AABB(position, size.X, size.Y);
        }

        protected AABB bounds;

        public AABB Bounds
        {
            get { return bounds; }
        }

        public Vector2 Position
        {
            get { return bounds.Center; }
            set
            {
                //bounds.X = value.X;
                //bounds.Y = value.Y;
            }
        }

        protected AABB previousBounds;
        public AABB PreviousBounds { get { return previousBounds; } }
        public Vector2 PreviousPosition { get { return PreviousBounds.Center; } }

        public event EventHandler BoundsChanged;

        private float rotation;

        public float Rotation
        {
            get { return rotation; }
            protected set { rotation = value % 360; }
        }

        public virtual void LoadContent()
        {
            bounds = new AABB(Bounds.Origin, SpriteWidth, SpriteHeight);
            Game.Actors.Insert(this);
        }

        public Texture2D Texture { get; set; }
        public bool Visible { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, rotation, new Vector2(Bounds.HalfWidth, Bounds.HalfHeight), 1f,
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

        public virtual void Update(GameTime gameTime)
        {
            //Move(gameTime);
            OnObjectCollision(Game.Actors.Query(Bounds));
        }

        protected void RaiseBoundsChanged()
        {
            EventHandler handler = BoundsChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }

        protected virtual void OnObjectCollision(List<GameObject> collisions)
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
                if (se.X - pse.X > 0)
                    bounds = new AABB(new Vector2(se.X * Game.Map.TileWidth - (SpriteWidth + 1), Bounds.Y), SpriteWidth, SpriteHeight);
                // Wall located West
                else if (nw.X - pnw.X < 0)
                    bounds = new AABB(new Vector2(pnw.X * Game.Map.TileWidth + 1, Bounds.Y), SpriteWidth, SpriteHeight);
            // Check vertical
            if (!Game.Map.Query(new AABB(new Vector2(PreviousBounds.X, Bounds.Y), SpriteWidth, SpriteHeight)))
                // Wall located South
                if (se.Y - pse.Y > 0)
                    bounds = new AABB(new Vector2(Bounds.X, se.Y * Game.Map.TileHeight - (SpriteHeight + 1)), SpriteWidth, SpriteHeight);
                // Wall located North
                else if (nw.Y - pnw.Y < 0)
                    bounds = new AABB(new Vector2(Bounds.X, pnw.Y * Game.Map.TileHeight + 1), SpriteWidth, SpriteHeight);
        }
    }
}
