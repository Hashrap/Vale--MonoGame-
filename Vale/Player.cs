using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Vale
{
    class Player : GameActor
    {
        private bool controllable = true;

        public void Initialize(Texture2D texture)
        {
            base.Initialize(texture, new Vector2(100, 100));
            Speed = 300;
        }

        public void Update(GameTime gameTime)
        {
            if(controllable)
            {
                Position += Vector2.Multiply(Input.getVector(), (float)gameTime.ElapsedGameTime.TotalSeconds * speed);
            }
            base.Update();
            if (Input.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P))
                Console.WriteLine(Position.X+" "+Position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
