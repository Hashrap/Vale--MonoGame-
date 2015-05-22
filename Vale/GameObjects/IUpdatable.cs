using Microsoft.Xna.Framework;

namespace Vale.GameObjects
{
    /// <summary>
    /// Represents an object that needs to update itself constantly.
    /// </summary>
    public interface IUpdatable
    {
        void Update(GameTime gameTime);
    }
}
