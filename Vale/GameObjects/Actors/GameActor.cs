using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Skills;

namespace Vale.GameObjects.Actors
{
    class GameActor : IUpdatable
    {
        private List<Skill> skillSet; // might not be the best way to store skills?

        public Vector2 Position { get; set; }

        public Vector2 PreviousPosition { get; set; }

        public float Speed { get; set; }

        protected Texture2D texture;

        public void Initialize(Texture2D tex, Vector2 pos)
        {
            texture = tex;
            Position = pos;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public void ExecuteSkill(int skillIndex, params object[] list)
        {
            skillSet[skillIndex].Execute(list);
        }
    }
}
