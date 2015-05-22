using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.GameObjects.Actors
{
    class Hero : GameActor
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
                Position += Vector2.Multiply(Input.Input.getInput(), (float)gameTime.ElapsedGameTime.TotalSeconds * Speed);
            }
            base.Update(gameTime);
            if (Input.Input.KeyPress('P'))
                Console.WriteLine("pX:" + Position.X + " pY:" + Position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
