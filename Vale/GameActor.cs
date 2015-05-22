using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale
{
    class GameActor : IUpdatable
    {
        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        protected Vector2 previousPosition;
        public Vector2 PreviousPosition
        {
            get { return previousPosition; }
            set { previousPosition = value; }
        }
        protected float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        protected Texture2D texture;

        public void Initialize(Texture2D tex, Vector2 pos)
        {
            texture = tex;
            position = pos;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
