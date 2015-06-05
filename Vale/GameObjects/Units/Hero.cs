using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Vale.Control;
using Vale.GameObjects.Skills;
using System.Reflection;
using Microsoft.Xna.Framework.Input;

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
            Input input = Input.Instance;
            Vector2 mousePosition = this.Screen.camera.ScreenToWorldCoords(input.MousePosition);

            if (Controllable)
            {
                Velocity = Vector2.Multiply(Input.Instance.NormalizedInput, Speed);

                //make Player handle this. map skills to Commands "XCommand triggers attack1", "BCommand triggers attack 2" etc.
                if (input.MouseButtonPress("Left"))
                {
                    SkillOne.Execute(mousePosition + Vector2.One);
                }

                if (input.MouseButtonPress("Right"))
                {
                    SkillTwo.Execute(mousePosition);
                }

                if (input.KeyPress(' '))
                {
                    SkillThree.Execute(mousePosition);
                }

                Rotation = (float)Math.Atan2(mousePosition.Y - Position.Y, mousePosition.X - Position.X);
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

        public Hero(Vale.ScreenSystem.Screens.GameplayScreen gameScreen, Faction alignment = Faction.Player)
            : base(gameScreen, alignment)
        {
            Position = new Vector2(0, 0);
            Speed = 0.3f;

            SkillOne = new QuickShot(gameScreen, this);
            SkillTwo = new SplitShot(gameScreen, this);
            SkillThree = new ReturnShot(gameScreen, this);
        }
    }
}