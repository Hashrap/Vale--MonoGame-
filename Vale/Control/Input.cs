using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Vale.GameObjects;

namespace Vale.Control
{
    public class Input : IUpdate
    {
        #region Singleton
        private Input() { }
        public static Input Instance { get { return Nested.instance; } }
        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly Input instance = new Input();
        }
        #endregion

        public enum Mode
        {
            KeyboardMouse,
            GamePad
        };

        public enum MouseButtons
        {
            Left,
            Right,
            Middle,
            X1,
            X2
        };

        public GamePadState CurrentGamePadState { get; private set; }

        public KeyboardState CurrentKeyboardState { get; private set; }

        public MouseState CurrentMouseState { get; private set; }

        public GamePadState PreviousGamePadState { get; private set; }

        public KeyboardState PreviousKeyboardState { get; private set; }

        public MouseState PreviousMouseState { get; private set; }

        public Mode InputMode { get; set; }

        /// <summary>
        ///     X and Y input, clamped to a max length of 1.
        /// </summary>
        /// <returns>Vector2 containing X and Y axis input clamped to a length of 1</returns>
        public Vector2 NormalizedInput
        {
            get
            {
                var vector = RawInput;
                return vector.Length() <= 1 ? vector : Vector2.Normalize(vector);
            }
        }

        /// <summary>
        ///     Unclamped X and Y input
        /// </summary>
        /// <returns>Vector2 containing X and Y axis input.</returns>
        public Vector2 RawInput
        {
            get { return new Vector2(XAxisInput, YAxisInput); }
        }

        /// <summary>
        ///     Calculates horizontal axis input based on input mode.
        /// </summary>
        /// <returns>Float between -1 (left) and 1 (right).  0 indicates no input.</returns>
        public float XAxisInput
        {
            get
            {
                if (InputMode != Mode.KeyboardMouse)
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
        ///     Calculates vertical axis input based on input mode.
        /// </summary>
        /// <returns>Float between -1 (up) and 1 (down).  0 indicates no input.</returns>
        public float YAxisInput
        {
            get
            {
                if (InputMode != Mode.KeyboardMouse)
                    return CurrentGamePadState.ThumbSticks.Left.Y * -1;

                float y = 0;
                if (CurrentKeyboardState.IsKeyDown(Keys.W))
                    y -= 1;
                if (CurrentKeyboardState.IsKeyDown(Keys.S))
                    y += 1;

                return y;
            }
        }

        public void Initialize(Mode mode)
        {
            InputMode = mode;
        }

        public void Update(GameTime gameTime)
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

            #endregion Debug strings

            if (KeyPress(Keys.G))
            {
                if (InputMode == Mode.KeyboardMouse)
                {
                    InputMode = Mode.GamePad;
                    Console.WriteLine("GAMEPAD CONTROL");
                }
                else
                {
                    InputMode = Mode.KeyboardMouse;
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

        #region Keyboard

        /// <summary>
        ///     Check if keyboard key is currently down
        /// </summary>
        /// <param name="key">Input.Keys object to check</param>
        /// <returns>true if key is currently pressed, false if released</returns>
        public bool KeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        ///     Check if keyboard key is currently down
        /// </summary>
        /// <param name="key">char to cast as an Input.Keys object</param>
        /// <returns>result of passing cast to KeyDown(Keys)</returns>
        public bool KeyDown(char key)
        {
            return KeyDown((Keys)char.ToUpper(key));
        }

        /// <summary>
        ///     Check if keyboard key is being pressed
        /// </summary>
        /// <param name="key">Input.Keys object to check</param>
        /// <returns>true if key has just been pressed, false otherwise.</returns>
        public bool KeyPress(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        ///     Check if keyboard key is being pressed
        /// </summary>
        /// <param name="key">char to cast as an Input.Keys object</param>
        /// <returns>result of passing cast to KeyPress(Keys)</returns>
        public bool KeyPress(char key)
        {
            return KeyPress((Keys)char.ToUpper(key));
        }

        /// <summary>
        ///     Check if keyboard key is being released
        /// </summary>
        /// <param name="key">Input.Keys object to check</param>
        /// <returns>true if key has just been released, false otherwise. </returns>
        public bool KeyRelease(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        ///     Check if keyboard key is being released.
        /// </summary>
        /// <param name="key">char to cast as an Input.Keys object</param>
        /// <returns>result of passing cast to KeyRelease(Keys)</returns>
        public bool KeyRelease(char key)
        {
            return KeyRelease((Keys)char.ToUpper(key));
        }

        /// <summary>
        ///     Check if keyboard key is currently up
        /// </summary>
        /// <param name="key">Input.Keys object to check</param>
        /// <returns>true if key is currently released, false if pressed.</returns>
        public bool KeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        ///     Check if keyboard key is currently up
        /// </summary>
        /// <param name="key">char to cast as an Input.Keys object</param>
        /// <returns>result of passing cast to KeyUp(Keys)</returns>
        public bool KeyUp(char key)
        {
            return KeyUp((Keys)char.ToUpper(key));
        }

        #endregion Keyboard

        #region Gamepad

        /// <summary>
        ///     Checks if a gamepad button is currently down
        /// </summary>
        /// <param name="button">Input.Buttons object to check</param>
        /// <returns>true if button is currently pressed, false if released</returns>
        public bool ButtonDown(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button);
        }

        /// <summary>
        ///     Check if a gamepad button is currently down.
        /// </summary>
        /// <param name="button">int to cast as an Input.Buttons</param>
        /// <returns>result of passing the cast to ButtonDown(Buttons)</returns>
        public bool ButtonDown(int button)
        {
            return ButtonDown((Buttons)button);
        }

        /// <summary>
        ///     Checks if a gamepad button is being pressed
        /// </summary>
        /// <param name="button">Input.Buttons object to check</param>
        /// <returns>true if button has just been pressed, false otherwise</returns>
        public bool ButtonPress(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
        }

        /// <summary>
        ///     Check if a gamepad button is being pressed.
        /// </summary>
        /// <param name="button">int to cast as an Input.Buttons</param>
        /// <returns>result of passing the cast to ButtonPress(Buttons)</returns>
        public bool ButtonPress(int button)
        {
            return ButtonPress((Buttons)button);
        }

        /// <summary>
        ///     Checks if a gamepad button is being released
        /// </summary>
        /// <param name="button">Input.Buttons object to check</param>
        /// <returns>true if button has just been released, false otherwise</returns>
        public bool ButtonRelease(Buttons button)
        {
            return CurrentGamePadState.IsButtonUp(button) && PreviousGamePadState.IsButtonDown(button);
        }

        /// <summary>
        ///     Check if a gamepad button is being released.
        /// </summary>
        /// <param name="button">int to cast as an Input.Buttons</param>
        /// <returns>result of passing the cast to ButtonRelease(Buttons)</returns>
        public bool ButtonRelease(int button)
        {
            return ButtonRelease((Buttons)button);
        }

        /// <summary>
        ///     Checks if a gamepad button is currently up
        /// </summary>
        /// <param name="button">Input.Buttons object to check</param>
        /// <returns>true if button is currently released, false if pressed</returns>
        public bool ButtonUp(Buttons button)
        {
            return CurrentGamePadState.IsButtonUp(button);
        }

        /// <summary>
        ///     Check if a gamepad button is currently up.
        /// </summary>
        /// <param name="button">int to cast as an Input.Buttons</param>
        /// <returns>result of passing the cast to ButtonUp(Buttons)</returns>
        public bool ButtonUp(int button)
        {
            return ButtonUp((Buttons)button);
        }

        #endregion Gamepad

        #region Mouse & Cursor

        /// <summary>
        ///     Calculates the position of the mouse cursor
        /// </summary>
        /// <returns>current position of the mouse cursor as a Vector2(X,Y)</returns>
        public Vector2 MousePosition
        {
            get { return new Vector2(MouseX, MouseY); }
        }

        /// <summary>
        ///     Calculates X position of the mouse cursor
        /// </summary>
        /// <returns>current position of the mouse cursor on the X axis</returns>
        public int MouseX
        {
            get { return CurrentMouseState.X; }
        }

        /// <summary>
        ///     Calculates Y position of the mouse cursor
        /// </summary>
        /// <returns>current position of the mouse cursor on the Y axis</returns>
        public int MouseY
        {
            get { return CurrentMouseState.Y; }
        }

        /// <summary>
        ///     Check if a mouse button is currently down
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>true if the button is down</returns>
        public bool MouseButtonDown(MouseButtons button)
        {
            return MouseButtonCurrentlyDown(button);
        }

        /// <summary>
        ///     Check if a mouse button is currently down.
        /// </summary>
        /// <param name="button">String to cast as a MouseButton object.</param>
        /// <returns>Result of passing the cast to MouseButtonDown(MouseButton).</returns>
        public bool MouseButtonDown(string button)
        {
            return MouseButtonDown((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        ///     Check if a mouse button is being pressed
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>Returns true if the button has just been pressed, false otherwise</returns>
        public bool MouseButtonPress(MouseButtons button)
        {
            return MouseButtonCurrentlyDown(button) && !MouseButtonPreviouslyDown(button);
        }

        /// <summary>
        ///     Check if a mouse button is being pressed.
        /// </summary>
        /// <param name="button">String to cast as a MouseButton object.</param>
        /// <returns>Result of passing the cast to MouseButtonPress(MouseButton).</returns>
        public bool MouseButtonPress(string button)
        {
            return MouseButtonPress((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        ///     Check if a mouse button is being released
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>Returns true if the button has just been released, false otherwise</returns>
        public bool MouseButtonRelease(MouseButtons button)
        {
            return !MouseButtonCurrentlyDown(button) && MouseButtonPreviouslyDown(button);
        }

        /// <summary>
        ///     Check if a mouse button is being released.
        /// </summary>
        /// <param name="button">String to cast as a MouseButton object.</param>
        /// <returns>Result of passing the cast to MouseButtonRelease(MouseButton).</returns>
        public bool MouseButtonRelease(string button)
        {
            return MouseButtonRelease((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        ///     Check if a mouse button is currently up
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>true if the button is up</returns>
        public bool MouseButtonUp(MouseButtons button)
        {
            return !MouseButtonCurrentlyDown(button);
        }

        /// <summary>
        ///     Check if a mouse button is currently up.
        /// </summary>
        /// <param name="button">String to cast as a MouseButton object.</param>
        /// <returns>Result of passing the cast to MouseButtonUp(MouseButton).</returns>
        public bool MouseButtonUp(string button)
        {
            return MouseButtonUp((MouseButtons)Enum.Parse(typeof(MouseButtons), button));
        }

        /// <summary>
        ///     Check if a mouse button is currently down
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>true if the button is down</returns>
        private bool MouseButtonCurrentlyDown(MouseButtons button)
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
        ///     Check if a mouse button was previously down
        /// </summary>
        /// <param name="button">MouseButton object to check</param>
        /// <returns>true if the button was down last frame</returns>
        private bool MouseButtonPreviouslyDown(MouseButtons button)
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

        #endregion Mouse & Cursor
    }
}