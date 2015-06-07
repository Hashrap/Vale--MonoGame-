using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects
{
    public class KeyboardProvider : GameObject
    {
        public KeyboardProvider(GameplayScreen game)
            : base(game)
        {
        }

        public bool KeyPress(char key)
        {
            return Vale.Control.Input.Instance.KeyPress((Keys)char.ToUpper(key));
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
