using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.DrawingSystem
{
    /// <summary>
    /// The sprite struct is very dumb and simply contains a texture and the
    /// origin of the texture. The origin is the center of the Texture.
    /// </summary>
    public struct Sprite
    {
        /// <summary>
        /// The origin of the sprite. This gets set to the center of the texture
        /// upon construction.
        /// </summary>
        public Vector2 Origin;
        /// <summary>
        /// The sprite texture.
        /// </summary>
        public Texture2D Texture;

        public Sprite(Texture2D texture, Vector2 origin)
        {
            this.Texture = texture;
            this.Origin = origin;
        }

        public Sprite(Texture2D sprite)
        {
            Texture = sprite;
            Origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }
    }
}
