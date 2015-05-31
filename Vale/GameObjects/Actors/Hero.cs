using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Vale.Control;
using Vale.GameObjects.Skills;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    ///     The player-controlled Hero
    /// </summary>
    internal class Hero : CombatUnit
    {
        private Skill SkillOne, SkillTwo, SkillThree;

        public override void LoadContent()
        {
            ContentManager content = new ContentManager(Screen.ScreenManager.Game.Services, "Content");
            this.texture = content.Load<Texture2D>("Art/arrow20x20.png");
        }

        public override void Update(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine("Updating. The hero.");
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
            System.Diagnostics.Debug.WriteLine("Drawing the hero :)");
            base.Draw(gameTime);

            SkillOne.Draw(gameTime);
            SkillTwo.Draw(gameTime);
            SkillThree.Draw(gameTime);
        }

        public Hero(Vale.ScreenSystem.GameScreen gameScreen)
            : base(gameScreen)
        {
            Position = new Vector2(100f, 100f);
            Speed = 0.3f;

            SkillOne = new QuickShot(gameScreen, this);
            SkillTwo = new SplitShot(gameScreen, this);
            SkillThree = new ReturnShot(gameScreen, this);
        }
    }
}