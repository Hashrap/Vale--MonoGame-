using Microsoft.Xna.Framework;

namespace Vale.GameObjects
{
    /// <summary>
    ///     Represents an object that needs to update itself constantly.
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        ///     All classes that inherit this interface must update themselves based on game time.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        void Update(GameTime gameTime);
    }
}