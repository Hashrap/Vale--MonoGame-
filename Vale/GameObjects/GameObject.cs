using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.ScreenSystem.Screens;
using Vale.GameObjects.Collision;

namespace Vale.GameObjects
{
    public abstract class GameObject : IDraw, IUpdate, ICollide
    {
        public GameplayScreen Game { get; set; }

        public GameObject(GameplayScreen game)
        {   this.Game = game;  }

        public AABB Rect;
        public AABB Bounds { get { return Rect; } }
        public event EventHandler BoundsChanged;

        public virtual void LoadContent(ContentManager content)
        {
            // Nothing~
        }

        public virtual void UnloadContent(ContentManager content)
        {
            // Nothing~
        }

        public virtual void Update(GameTime gameTime)
        {
            // Nothing~
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Nothing~
        }
    }
}
