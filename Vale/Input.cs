using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Vale
{
    static class Input
    {
        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;

        public static GamePadState currentGamePadState;
        public static GamePadState previousGamePadState;

        public static MouseState currentMouseState;
        public static MouseState previousMouseState;

        public static void Update()
        {
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
            previousGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentGamePadState = GamePad.GetState(0);
        }
    }
}
