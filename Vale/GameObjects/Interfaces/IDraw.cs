using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Vale.GameObjects
{
    internal interface IDraw
    {
        Texture2D Texture { get; set; }
        bool Visible { get; set; }
        int SpriteWidth { get; set; }
        int SpriteHeight { get; set; }

        //void LoadContent();
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}