using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Vale.Control;
using Vale.GameObjects.Skills;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    ///     The player-controlled Hero
    /// </summary>
    internal class Hero : GameActor
    {
        private Skill SkillOne, SkillTwo, SkillThree;

        public void Initialize(Texture2D texture)
        {
            base.Initialize(texture, new Vector2(100, 100));
            Speed = 0.3f;
        }

        public override void Update(GameTime gameTime)
        {
            Input input = Input.Instance;

            if (Controllable)
            {
                Velocity = Vector2.Multiply(Input.Instance.NormalizedInput, Speed);

                //make Player handle this. map skills to Commands "XCommand triggers attack1", "BCommand triggers attack 2" etc.
                if (input.MouseButtonPress("Left"))
                {
                    SkillOne.Execute(input.MousePosition + Vector2.One);
                }

                if (input.MouseButtonPress("Right"))
                {
                    SkillTwo.Execute(input.MousePosition);
                }

                if (input.KeyPress(' '))
                {
                    SkillThree.Execute(input.MousePosition);
                }

                Rotation = (float)Math.Atan2(input.MouseY - Position.Y, input.MouseX - Position.X);
            }

            SkillOne.Update(gameTime);
            SkillTwo.Update(gameTime);
            SkillThree.Update(gameTime);

            if (input.KeyPress('P'))
                Console.WriteLine("pX:" + Position.X + " pY:" + Position.Y);

            // always call base
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SkillOne.Draw(gameTime);
            SkillTwo.Draw(gameTime);
            SkillThree.Draw(gameTime);
        }

        public Hero(Game1 game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            SkillOne = new QuickShot(game, spriteBatch, this);
            SkillTwo = new SplitShot(game, spriteBatch, this);
            SkillThree = new ReturnShot(game, spriteBatch, this);
        }
    }
}