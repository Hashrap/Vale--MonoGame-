using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale
{
    class GameActor
    {
        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Texture2D texture;

        public void Initialize(Texture2D tex, Vector2 pos)
        {
            texture = tex;
            Position = pos;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
