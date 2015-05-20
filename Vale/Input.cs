﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Vale
{
    static class Input
    {
        public enum Mode { KBAM, Pad };
        private static Mode inputMode;
        public static Mode InputMode
        {
            get { return inputMode; }
            set { inputMode = value; }
        }

        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;
        private static GamePadState currentGamePadState;
        private static GamePadState previousGamePadState;
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        #region Input State Properties
        public static KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }
        public static KeyboardState PreviousKeyboardState
        {
            get { return previousKeyboardState; }
        }

        public static GamePadState CurrentGamePadState
        {
            get { return currentGamePadState; }
        }
        public static GamePadState PreviousGamePadState
        {
            get { return previousGamePadState; }
        }

        public static MouseState CurrentMouseState
        {
            get { return currentMouseState; }
        }                      
        public static MouseState PreviousMouseState
        {
            get { return previousMouseState; }
        }
        #endregion

        public static void Initialize(Mode mode)
        {
            inputMode = mode;
        }

        #region Helper Functions
        public static float getY()
        {
            float y = 0;
            if(inputMode == Mode.KBAM)
            {
                if (Input.CurrentKeyboardState.IsKeyDown(Keys.W))
                    y -= 1;
                if (Input.CurrentKeyboardState.IsKeyDown(Keys.S))
                    y += 1;
            }
            else
            {
                y = Input.CurrentGamePadState.ThumbSticks.Left.Y;
            }
            return y;
        }

        public static float getX()
        {
            float x = 0;
            if (inputMode == Mode.KBAM)
            {
                if (Input.CurrentKeyboardState.IsKeyDown(Keys.D))
                    x += 1;
                if (Input.CurrentKeyboardState.IsKeyDown(Keys.A))
                    x -= 1;
            }
            else
            {
                x = Input.CurrentGamePadState.ThumbSticks.Left.X;
            }
            return x;
        }

        public static Vector2 getRawVector()
        { return new Vector2(getX(), getY()); }
        
        public static Vector2 getVector()
        {
            Vector2 vector = getRawVector();
            if (vector == Vector2.Zero)
                return Vector2.Zero;
            return Vector2.Normalize(vector);
        }

        public static bool KeyDown(Keys key)
        { return CurrentKeyboardState.IsKeyDown(key); }
        public static bool KeyUp(Keys key)
        { return CurrentKeyboardState.IsKeyUp(key); }
        public static bool KeyRelease(Keys key)
        { return CurrentKeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key); }
        public static bool KeyPress(Keys key)
        { return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key); }
        #endregion

        public static void Update()
        {
            #region Debug strings
            if (Input.KeyPress(Keys.I))
            {
                Console.WriteLine("X:"+Input.getX()+" Y:"+Input.getY());
                Console.WriteLine("Raw:"+getRawVector().ToString());
                Console.WriteLine("Normalized:"+getVector().ToString());
            }
            if (Input.KeyPress(Keys.K))
                Console.WriteLine("K Pressed");
            if (Input.KeyRelease(Keys.K))
                Console.WriteLine("K Released");
            #endregion

            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
            previousGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentGamePadState = GamePad.GetState(0);
        }
    }
}
