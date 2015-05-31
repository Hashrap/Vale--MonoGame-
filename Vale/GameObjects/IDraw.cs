using Microsoft.Xna.Framework;
using System;

namespace Vale.GameObjects
{
    internal interface IDraw
    {
        //void LoadContent();
        void Draw(GameTime gameTime);
    }
}