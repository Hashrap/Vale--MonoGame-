using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects
{
    public class KeyboardProvider : IUpdate
    {
        public KeyboardProvider(GameplayScreen game)
        {
        }

        public bool KeyPress(char key)
        {
            return Vale.Control.Input.Instance.KeyPress((Keys)char.ToUpper(key));
        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
