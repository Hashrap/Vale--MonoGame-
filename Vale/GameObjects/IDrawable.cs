using Microsoft.Xna.Framework.Graphics;

namespace Vale.GameObjects
{
    /// <summary>
    ///     Represents an object that has a sprite or graphic associated with it.
    /// </summary>
    internal interface IDrawable
    {
        /// <summary>
        ///     All classes inheriting this interface must be able to draw.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}