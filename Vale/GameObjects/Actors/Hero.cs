using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Skills;
using Input = Vale.Control.Input;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    /// The player-controlled Hero
    /// </summary>
    class Hero : GameActor
    {
        private Skill SkillOne, SkillTwo, SkillThree;

        public void Initialize(Texture2D texture)
        {
            base.Initialize(texture, new Vector2(100, 100));
            Speed = .3;
        }

        public override void Update(GameTime gameTime)
        {
            if (Controllable)
            {
                Velocity = Vector2.Multiply(Input.NormalizedInput, (float) Speed);
                // input should be handled by Player class maybe? Player moves the hero

                if (Input.MouseButtonPress("Left")) //make Player handle this. map skills to Commands "XCommand triggers attack1", "BCommand triggers attack 2" etc.
                {
                    SkillOne.Execute(Input.MousePosition);
                }
                if (Input.MouseButtonPress("Right")) //make Player handle this. map skills to Commands "XCommand triggers attack1", "BCommand triggers attack 2" etc.
                {
                    SkillTwo.Execute(Input.MousePosition);
                }
                if (Input.KeyPress(' ')) //make Player handle this. map skills to Commands "XCommand triggers attack1", "BCommand triggers attack 2" etc.
                {
                    SkillThree.Execute(Input.MousePosition);
                }
            }

            SkillOne.Update(gameTime);
            SkillTwo.Update(gameTime);
            SkillThree.Update(gameTime);
            
            if (Input.KeyPress('P'))
                Console.WriteLine("pX:" + Position.X + " pY:" + Position.Y);

            // always call base
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            //do we need to draw anything special for the Hero? if not, delegate drawing to parent.
            base.Draw(gameTime);

            SkillOne.Draw(gameTime);
            SkillTwo.Draw(gameTime);
            SkillThree.Draw(gameTime);
        }

        public Hero(Game1 game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            SkillOne = new QuickShot(game, spriteBatch, this);
            SkillTwo = new SplitShot(game, spriteBatch, this);
            SkillThree = new ReturnShot(game, spriteBatch, this);
        }
    }
}
