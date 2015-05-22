using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vale.Input
{
    public static class Input
    {
        public static void Initialize(Mode mode)
        {
            InputMode = mode;
        }

        public static void Update()
        {
            #region Debug strings

            if (KeyPress('I'))
            {
                Console.WriteLine("X-axis:" + GetX() + " Y-axis:" + GetY());
                Console.WriteLine("Raw:" + GetRawInput());
                Console.WriteLine("Normalized:" + GetInput());
            }
            if (KeyPress('M'))
                Console.WriteLine("mX:" + MouseX() + " mY:" + MouseY());
            if (KeyPress('K'))
                Console.WriteLine("K Pressed");
            if (KeyRelease('K'))
                Console.WriteLine("K Released");
            if (ButtonPress(16))
                Console.WriteLine("Start Pressed");
            if (ButtonRelease(16))
                Console.WriteLine("Start Released");
            if (MouseButtonPress("Middle"))
                Console.WriteLine("MMB Pressed");
            if (MouseButtonRelease("Middle"))
                Console.WriteLine("MMB Released");

            #endregion

            if (KeyPress(Keys.G))
            {
                if (InputMode == Mode.KBAM)
                {
                    InputMode = Mode.Pad;
                    Console.WriteLine("GAMEPAD CONTROL");
                }
                else
                {
                    InputMode = Mode.KBAM;
                    Console.WriteLine("KEYBOARD CONTROL");
                }
            }

            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;
            PreviousGamePadState = CurrentGamePadState;

            //polling
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
            CurrentGamePadState = GamePad.GetState(0);
        }

        #region Attributes

        public enum Mode
        {
            KBAM,
            Pad
        };

        public static Mode InputMode { get; set; }

        public enum MouseButtons
        {
            Left,
            Right,
            Middle,
            X1,
            X2
        };

        //Polling data, stored in an [input]State wrapper

        #region Polling Data Properties

        public static KeyboardState CurrentKeyboardState { get; private set; }

        public static KeyboardState PreviousKeyboardState { get; private set; }

        public static GamePadState CurrentGamePadState { get; private set; }

        public static GamePadState PreviousGamePadState { get; private set; }

        public static MouseState CurrentMouseState { get; private set; }

        public static MouseState PreviousMouseState { get; private set; }

        #endregion

        #endregion

        #region Tools

        /// <summary>
        ///  Calculates vertical axis input based on input mode.
        /// Returns a float between -1 (up) and 1 (down).  0 indicates no input.
        /// </summary>
        /// <returns></returns>
        public static float GetY()
        {
            float y = 0;
            if (InputMode == Mode.KBAM)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.W))
                    y -= 1;
                if (CurrentKeyboardState.IsKeyDown(Keys.S))
                    y += 1;
            }
            else
            {
                y = CurrentGamePadState.ThumbSticks.Left.Y * -1;
            }
            return y;
        }

        /// <summary>
        /// Calculates horizontal axis input based on input mode.
        /// Returns a float between -1 (left) and 1 (right).  0 indicates no input.
        /// </summary>
        /// <returns></returns>
        public static float GetX()
        {
            float x = 0;
            if (InputMode == Mode.KBAM)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.D))
                    x += 1;
                if (CurrentKeyboardState.IsKeyDown(Keys.A))
                    x -= 1;
            }
            else
            {
                x = CurrentGamePadState.ThumbSticks.Left.X;
            }
            return x;
        }

        /// <summary>
        /// Returns a Vector2 containing X and Y axis input.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetRawInput()
        {
            return new Vector2(GetX(), GetY());
        }

        /// <summary>
        /// Normalizes axis input values if the length of the vector exceeds 1
        /// Returns a Vector2 containing X and Y axis input clamped to a length of 1
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetInput()
        {
            Vector2 vector = GetRawInput();
            return vector.Length() <= 1 ? vector : Vector2.Normalize(vector);
        }

        #region Keyboard

        /// <summary>
        ///  Accepts an Input.Keys object to check
        /// Returns true if key is currently pressed, false if released
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Accepts an Input.Keys object to check
        /// Returns true if key is currently released, false if pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Accepts an Input.Keys object to check
        /// Returns true if key has just been pressed, false otherwise 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyPress(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        ///  Accepts an Input.Keys object to check
        /// Returns true if key has just been released, false otherwise 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyRelease(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        ///  Accepts a character to cast as an Input.Keys object
        /// Returns the result of passing the object to KeyDown(Keys)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyDown(char key)
        {
            return KeyDown((Keys)char.ToUpper(key));
        }

        /// <summary>
        ///  Accepts a character to cast as an Input.Keys object
        /// Returns the result of passing the object to KeyUp(Keys)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyUp(char key)
        {
            return KeyUp((Keys)char.ToUpper(key));
        }

        /// <summary>
        /// Accepts a character to cast as an Input.Keys object
        /// Returns the result of passing the object to KeyPress(Keys)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyPress(char key)
        {
            return KeyPress((Keys)char.ToUpper(key));
        }

        /// <summary>
        ///  Accepts a character to cast as an Input.Keys object
        /// Returns the result of passing the object to KeyRelease(Keys)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyRelease(char key)
        {
            return KeyRelease((Keys)char.ToUpper(key));
        }

        #endregion

        #region Gamepad

        /// <summary>
        /// Accepts an Input.Buttons object to check
        /// Returns true if button is currently pressed, false if released
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool ButtonDown(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button);
        }

        /// <summary>
        ///  Accepts an Input.Buttons object to check
        /// Returns true if button is currently released, false if pressed
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool ButtonUp(Buttons button)
        {
            return CurrentGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Accepts an Input.Buttons object to check
        /// Returns true if button has just been pressed, false otherwise
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool ButtonPress(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Accepts an int to cast as an Input.Buttons object
        /// Returns true if button has just been released, false otherwise
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool ButtonRelease(Buttons button)
        {
            return CurrentGamePadState.IsButtonUp(button) && PreviousGamePadState.IsButtonDown(button);
        }

        /// <summary>
        ///  Accepts an int to cast as an Input.Buttons object
        /// Returns the result of passing the object to ButtonDown(Buttons)
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool ButtonDown(int button)
        {
            return ButtonDown((Buttons)button);
        }

        // Accepts an int to cast as an Input.Buttons object
        // Returns the result of passing the object to ButtonUp(Buttons)
        public static bool ButtonUp(int button)
        {
            return ButtonUp((Buttons)button);
        }

        /// <summary>
        /// Accepts an int to cast as an Input.Buttons object
        /// Returns the result of passing the object to ButtonPress(Buttons)
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool ButtonPress(int button)
        {
            return ButtonPress((Buttons)button);
        }

        /// <summary>
        /// Accepts an Input.Buttons object to check.  Returns the result of passing the object to ButtonRelease(Buttons)
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool ButtonRelease(int button)
        {
            return ButtonRelease((Buttons)button);
        }

        #endregion

        #region Mouse & Cursor

        /// <summary>
        /// Returns the current position of the mouse cursor on the X axis
        /// </summary>
        /// <returns></returns>
        public static int MouseX()
        {
            return CurrentMouseState.X;
        }

        /// <summary>
        /// Returns the current position of the mouse cursor on the Y axis
        /// </summary>
        /// <returns></returns>
        public static int MouseY()
        {
            return CurrentMouseState.Y;
        }

        /// <summary>
        ///  Returns the current position of the mouse cursor as a Vector2(X,Y)
        /// </summary>
        /// <returns></returns>
        public static Vector2 MousePos()
        {
            return new Vector2(MouseX(), MouseY());
        }

        /// <summary>
        /// Accepts a MouseButton to check
        /// </summary>
        /// <param name="button"></param>
        /// <returns> Returns true if the button is being pressed</returns>
        private static bool MouseButtonCurrentlyDown(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return CurrentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.Right:
                    return CurrentMouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.Middle:
                    return CurrentMouseState.MiddleButton == ButtonState.Pressed;
                case MouseButtons.X1:
                    return CurrentMouseState.XButton1 == ButtonState.Pressed;
                case MouseButtons.X2:
                    return CurrentMouseState.XButton2 == ButtonState.Pressed;
            }
            return false;
        }

        /// <summary>
        /// Accepts a MouseButton to check
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Returns true if the button was being pressed in the previous frame</returns>
        private static bool MouseButtonPreviouslyDown(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return PreviousMouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.Right:
                    return PreviousMouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.Middle:
                    return PreviousMouseState.MiddleButton == ButtonState.Pressed;
                case MouseButtons.X1:
                    return PreviousMouseState.XButton1 == ButtonState.Pressed;
                case MouseButtons.X2:
                    return PreviousMouseState.XButton2 == ButtonState.Pressed;
            }
            return false;
        }

        /// <summary>
        /// Accepts a MouseButton to check
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Returns true if the button is being pressed</returns>
        public static bool MouseButtonDown(MouseButtons button)
        {
            return MouseButtonCurrentlyDown(button);
        }

        /// <summary>
        /// Accepts a MouseButton to check
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Returns true if the button is not being pressed</returns>
        public static bool MouseButtonUp(MouseButtons button)
        {
            return !MouseButtonCurrentlyDown(button);
        }

        /// <summary>
        /// Accepts a MouseButton to check
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Returns true if the button has just been pressed, false otherwise</returns>
        public static bool MouseButtonPress(MouseButtons button)
        {
            return MouseButtonCurrentlyDown(button) && !MouseButtonPreviouslyDown(button);
        }
        /// <summary>
        /// Accepts a MouseButton to check
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Returns true if the button has just been released, false otherwise</returns>
        public static bool MouseButtonRelease(MouseButtons button)
        {
            return !MouseButtonCurrentlyDown(button) && MouseButtonPreviouslyDown(button);
        }

        /// <summary>
        /// Accepts a string to cast as a MouseButton object
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Returns the result of passing the object to MouseButtonDown(MouseButton)</returns>
        public static bool MouseButtonDown(string button)
        {
            return MouseButtonDown((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        /// Accepts a string to cast as a MouseButton object
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Returns the result of passing the object to MouseButtonUp(MouseButton)</returns>
        public static bool MouseButtonUp(string button)
        {
            return MouseButtonUp((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        /// Accepts a string to cast as a MouseButton object
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Returns the result of passing the object to MouseButtonPress(MouseButton)</returns>
        public static bool MouseButtonPress(string button)
        {
            return MouseButtonPress((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        ///     Accepts a string to cast as a MouseButton object
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns>Returns the result of passing the object to MouseButtonRelease(MouseButton)</returns>
        public static bool MouseButtonRelease(string button)
        {
            return MouseButtonRelease((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        #endregion

        #endregion
    }
}