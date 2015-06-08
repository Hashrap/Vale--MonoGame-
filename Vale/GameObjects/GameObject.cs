using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects
{
    public abstract class GameObject : IDraw, IUpdate
    {
        public GameplayScreen Game { get; set; }

        public GameObject(GameplayScreen game)
        {
            this.Game = game;
        }


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
