using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                Position += Vector2.Multiply(Input.getRawVector(), (float)gameTime.ElapsedGameTime.TotalSeconds * speed);
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
