using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.GameObjects
{
    /// <summary>
    /// Represents an object that has a sprite or graphic associated with it.
    /// </summary>
    interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch);
    }
}
