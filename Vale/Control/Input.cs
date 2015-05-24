using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vale.Control
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
                Console.WriteLine("X-axis:" + XAxisInput + " Y-axis:" + YAxisInput);
                Console.WriteLine("Raw:" + RawInput);
                Console.WriteLine("Normalized:" + NormalizedInput);
            }

            if (KeyPress('M'))
                Console.WriteLine("mX:" + MouseX + " mY:" + MouseY);
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
        /// </summary>
        /// <returns>Float between -1 (up) and 1 (down).  0 indicates no input.</returns>
        public static float YAxisInput
        {
            get
            {
                if (InputMode != Mode.KBAM)
                    return CurrentGamePadState.ThumbSticks.Left.Y * -1;

                float y = 0;
                if (CurrentKeyboardState.IsKeyDown(Keys.W))
                    y -= 1;
                if (CurrentKeyboardState.IsKeyDown(Keys.S))
                    y += 1;

                return y;
            }
        }

        /// <summary>
        /// Calculates horizontal axis input based on input mode.
        /// </summary>
        /// <returns>Float between -1 (left) and 1 (right).  0 indicates no input.</returns>
        public static float XAxisInput
        {
            get
            {
                if (InputMode != Mode.KBAM)
                    return CurrentGamePadState.ThumbSticks.Left.X;

                float x = 0;
                if (CurrentKeyboardState.IsKeyDown(Keys.D))
                    x += 1;
                if (CurrentKeyboardState.IsKeyDown(Keys.A))
                    x -= 1;

                return x;
            }
        }



        /// <summary>
        /// Unclamped X and Y input
        /// </summary>
        /// <returns>Vector2 containing X and Y axis input.</returns>
        public static Vector2 RawInput
        {
            get { return new Vector2(XAxisInput, YAxisInput); }
        }

        /// <summary>
        /// X and Y input, clamped to a max length of 1.
        /// </summary>
        /// <returns>Vector2 containing X and Y axis input clamped to a length of 1</returns>
        public static Vector2 NormalizedInput
        {
            get
            {
                var vector = RawInput;
                return vector.Length() <= 1 ? vector : Vector2.Normalize(vector);
            }
        }

        #region Keyboard

        /// <summary>
        /// Check if keyboard key is currently down
        /// </summary>
        /// <param name="key">Input.Keys object to check</param>
        /// <returns>true if key is currently pressed, false if released</returns>
        public static bool KeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Check if keyboard key is currently up
        /// </summary>
        /// <param name="key">Input.Keys object to check</param>
        /// <returns>true if key is currently released, false if pressed.</returns>
        public static bool KeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Check if keyboard key is being pressed
        /// </summary>
        /// <param name="key">Input.Keys object to check</param>
        /// <returns>true if key has just been pressed, false otherwise.</returns>
        public static bool KeyPress(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Check if keyboard key is being released
        /// </summary>
        /// <param name="key">Input.Keys object to check</param>
        /// <returns>true if key has just been released, false otherwise. </returns>
        public static bool KeyRelease(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Check if keyboard key is currently down 
        /// </summary>
        /// <param name="key">char to cast as an Input.Keys object</param>
        /// <returns>result of passing cast to KeyDown(Keys)</returns>
        public static bool KeyDown(char key)
        {
            return KeyDown((Keys)char.ToUpper(key));
        }

        /// <summary>
        /// Check if keyboard key is currently up 
        /// </summary>
        /// <param name="key">char to cast as an Input.Keys object</param>
        /// <returns>result of passing cast to KeyUp(Keys)</returns>
        public static bool KeyUp(char key)
        {
            return KeyUp((Keys)char.ToUpper(key));
        }

        /// <summary>
        /// Check if keyboard key is being pressed
        /// </summary>
        /// <param name="key">char to cast as an Input.Keys object</param>
        /// <returns>result of passing cast to KeyPress(Keys)</returns>
        public static bool KeyPress(char key)
        {
            return KeyPress((Keys)char.ToUpper(key));
        }

        /// <summary>
        /// Check if keyboard key is being released. 
        /// </summary>
        /// <param name="key">char to cast as an Input.Keys object</param>
        /// <returns>result of passing cast to KeyRelease(Keys)</returns>
        public static bool KeyRelease(char key)
        {
            return KeyRelease((Keys)char.ToUpper(key));
        }

        #endregion

        #region Gamepad

        /// <summary>
        /// Checks if a gamepad button is currently down
        /// </summary>
        /// <param name="button">Input.Buttons object to check</param>
        /// <returns>true if button is currently pressed, false if released</returns>
        public static bool ButtonDown(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks if a gamepad button is currently up
        /// </summary>
        /// <param name="button">Input.Buttons object to check</param>
        /// <returns>true if button is currently released, false if pressed</returns>
        public static bool ButtonUp(Buttons button)
        {
            return CurrentGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Checks if a gamepad button is being pressed
        /// </summary>
        /// <param name="button">Input.Buttons object to check</param>
        /// <returns>true if button has just been pressed, false otherwise</returns>
        public static bool ButtonPress(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Checks if a gamepad button is being released
        /// </summary>
        /// <param name="button">Input.Buttons object to check</param>
        /// <returns>true if button has just been released, false otherwise</returns>
        public static bool ButtonRelease(Buttons button)
        {
            return CurrentGamePadState.IsButtonUp(button) && PreviousGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Check if a gamepad button is currently down.
        /// </summary>
        /// <param name="button">int to cast as an Input.Buttons</param>
        /// <returns>result of passing the cast to ButtonDown(Buttons)</returns>
        public static bool ButtonDown(int button)
        {
            return ButtonDown((Buttons)button);
        }

        /// <summary>
        /// Check if a gamepad button is currently up.
        /// </summary>
        /// <param name="button">int to cast as an Input.Buttons</param>
        /// <returns>result of passing the cast to ButtonUp(Buttons)</returns>
        public static bool ButtonUp(int button)
        {
            return ButtonUp((Buttons)button);
        }

        /// <summary>
        /// Check if a gamepad button is being pressed.
        /// </summary>
        /// <param name="button">int to cast as an Input.Buttons</param>
        /// <returns>result of passing the cast to ButtonPress(Buttons)</returns>
        public static bool ButtonPress(int button)
        {
            return ButtonPress((Buttons)button);
        }

        /// <summary>
        /// Check if a gamepad button is being released.
        /// </summary>
        /// <param name="button">int to cast as an Input.Buttons</param>
        /// <returns>result of passing the cast to ButtonRelease(Buttons)</returns>
        public static bool ButtonRelease(int button)
        {
            return ButtonRelease((Buttons)button);
        }

        #endregion

        #region Mouse & Cursor

        /// <summary>
        /// Calculates X position of the mouse cursor
        /// </summary>
        /// <returns>current position of the mouse cursor on the X axis</returns>
        public static int MouseX { get { return CurrentMouseState.X; } }

        /// <summary>
        /// Calculates Y position of the mouse cursor
        /// </summary>
        /// <returns>current position of the mouse cursor on the Y axis</returns>
        public static int MouseY { get { return CurrentMouseState.Y; } }

        /// <summary>
        /// Calculates the position of the mouse cursor
        /// </summary>
        /// <returns>current position of the mouse cursor as a Vector2(X,Y)</returns>
        public static Vector2 MousePosition { get { return new Vector2(MouseX, MouseY); } }

        /// <summary>
        /// Check if a mouse button is currently down
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>true if the button is down</returns>
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
        /// Check if a mouse button was previously down
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>true if the button was down last frame</returns>
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
        /// Check if a mouse button is currently down
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>true if the button is down</returns>
        public static bool MouseButtonDown(MouseButtons button)
        {
            return MouseButtonCurrentlyDown(button);
        }

        /// <summary>
        /// Check if a mouse button is currently up
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>true if the button is up</returns>
        public static bool MouseButtonUp(MouseButtons button)
        {
            return !MouseButtonCurrentlyDown(button);
        }

        /// <summary>
        /// Check if a mouse button is being pressed
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>Returns true if the button has just been pressed, false otherwise</returns>
        public static bool MouseButtonPress(MouseButtons button)
        {
            return MouseButtonCurrentlyDown(button) && !MouseButtonPreviouslyDown(button);
        }

        /// <summary>
        /// Check if a mouse button is being released
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>Returns true if the button has just been released, false otherwise</returns>
        public static bool MouseButtonRelease(MouseButtons button)
        {
            return !MouseButtonCurrentlyDown(button) && MouseButtonPreviouslyDown(button);
        }

        /// <summary>
        /// Check if a mouse button is currently down.
        /// </summary>
        /// <param name="button">String to cast as a MouseButton object.</param>
        /// <returns>Result of passing the cast to MouseButtonDown(MouseButton).</returns>
        public static bool MouseButtonDown(string button)
        {
            return MouseButtonDown((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        /// Check if a mouse button is currently up.
        /// </summary>
        /// <param name="button">String to cast as a MouseButton object.</param>
        /// <returns>Result of passing the cast to MouseButtonUp(MouseButton).</returns>
        public static bool MouseButtonUp(string button)
        {
            return MouseButtonUp((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        /// Check if a mouse button is being pressed.
        /// </summary>
        /// <param name="button">String to cast as a MouseButton object.</param>
        /// <returns>Result of passing the cast to MouseButtonPress(MouseButton).</returns>
        public static bool MouseButtonPress(string button)
        {
            return MouseButtonPress((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        /// Check if a mouse button is being released.
        /// </summary>
        /// <param name="button">String to cast as a MouseButton object.</param>
        /// <returns>Result of passing the cast to MouseButtonRelease(MouseButton).</returns>
        public static bool MouseButtonRelease(string button)
        {
            return MouseButtonRelease((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        #endregion

        #endregion
    }
}