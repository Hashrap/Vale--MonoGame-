using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Vale.GameObjects
{
    internal interface IDraw
    {
        //void LoadContent();
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}