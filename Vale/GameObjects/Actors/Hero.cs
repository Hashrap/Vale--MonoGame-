using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    /// The player-controlled Hero
    /// </summary>
    class Hero : GameActor
    {
        public void Initialize(Texture2D texture)
        {
            base.Initialize(texture, new Vector2(100, 100));
            Speed = 300;
        }

        public override void Update(GameTime gameTime)
        {
            if (Controllable)
            {
                Position += Vector2.Multiply(Input.Input.GetInput(), (float)(gameTime.ElapsedGameTime.TotalSeconds * Speed));
                // input should be handled by Player class maybe? Player moves the hero
            }
            base.Update(gameTime);
            if (Input.Input.KeyPress('P'))
                Console.WriteLine("pX:" + Position.X + " pY:" + Position.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //do we need to draw anything special for the Hero? if not, delegate drawing to parent.
            base.Draw(spriteBatch);
        }
    }
}
