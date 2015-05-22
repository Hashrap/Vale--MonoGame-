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
                Position += Vector2.Multiply(Input.getInput(), (float)gameTime.ElapsedGameTime.TotalSeconds * speed);
            }
            base.Update();
            if (Input.KeyPress('P'))
                Console.WriteLine("pX:" + Position.X + " pY:" + Position.Y);
        }

        public void DoMainAttack()
        {
            //do the attack in your m1 slot
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
