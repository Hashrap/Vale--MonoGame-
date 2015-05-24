using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vale.GameObjects
{
    abstract class MoveableGameObject:IUpdateable
    {
        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; set; }
        public Vector2 Velocity { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            Move( gameTime);
        }

        private Vector2 Move(GameTime gameTime)
        {
            Position += (Velocity * gameTime.ElapsedGameTime.Milliseconds);

            return Position;
        }

        public bool Enabled { get; private set; }
        public int UpdateOrder { get; private set; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
    }
}
