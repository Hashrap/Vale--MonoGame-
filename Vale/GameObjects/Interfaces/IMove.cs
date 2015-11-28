using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using OpenTK;
using Vale.GameObjects.Collision;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Vale.GameObjects
{
    internal interface IMove
    {
        Vector2 Velocity { get; set; }
        event EventHandler ObjectMoved;
        event EventHandler ObjectStoppedMoving;
        Vector2 Move(GameTime gameTime);
    }
}
