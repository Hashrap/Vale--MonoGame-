using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vale.Input
{
    static class Input
    {
        #region Attributes
        public enum Mode { KBAM, Pad };
        private static Mode inputMode;
        public static Mode InputMode
        {
            get { return inputMode; }
            set { inputMode = value; }
        }

        public enum MouseButtons { Left, Right, Middle, X1, X2 };

        //Polling data, stored in an [input]State wrapper
        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;
        private static GamePadState currentGamePadState;
        private static GamePadState previousGamePadState;
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;
        #region Polling Data Properties
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
        #endregion

        public static void Initialize(Mode mode)
        {
            inputMode = mode;
        }

        #region Tools

        // Calculates vertical axis input based on input mode.
        // Returns a float between -1 (up) and 1 (down).  0 indicates no input.
        public static float getY()
        {
            float y = 0;
            if(inputMode == Mode.KBAM)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.W))
                    y -= 1;
                if (CurrentKeyboardState.IsKeyDown(Keys.S))
                    y += 1;
            }
            else
            {
                y = Input.CurrentGamePadState.ThumbSticks.Left.Y * -1;
            }
            return y;
        }

        // Calculates horizontal axis input based on input mode.
        // Returns a float between -1 (left) and 1 (right).  0 indicates no input.
        public static float getX()
        {
            float x = 0;
            if (inputMode == Mode.KBAM)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.D))
                    x += 1;
                if (CurrentKeyboardState.IsKeyDown(Keys.A))
                    x -= 1;
            }
            else
            {
                x = Input.CurrentGamePadState.ThumbSticks.Left.X;
            }
            return x;
        }

        // Returns a Vector2 containing X and Y axis input.
        public static Vector2 getRawInput()
        { return new Vector2(getX(), getY()); }

        // Normalizes axis input values if the length of the vector exceeds 1
        // Returns a Vector2 containing X and Y axis input clamped to a length of 1
        public static Vector2 getInput()
        {
            Vector2 vector = getRawInput();
            if (vector.Length() <= 1)
                return vector;
            return Vector2.Normalize(vector);
        }

        #region Keyboard
        // CASE INSENSITIVE

        // Accepts an Input.Keys object to check
        // Returns true if key is currently pressed, false if released
        public static bool KeyDown(Keys key)
        { return CurrentKeyboardState.IsKeyDown(key); }

        // Accepts an Input.Keys object to check
        // Returns true if key is currently released, false if pressed
        public static bool KeyUp(Keys key)
        { return CurrentKeyboardState.IsKeyUp(key); }

        // Accepts an Input.Keys object to check
        // Returns true if key has just been pressed, false otherwise 
        public static bool KeyPress(Keys key)
        { return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key); }

        // Accepts an Input.Keys object to check
        // Returns true if key has just been released, false otherwise 
        public static bool KeyRelease(Keys key)
        { return CurrentKeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key); }

        // Accepts a character to cast as an Input.Keys object
        // Returns the result of passing the object to KeyDown(Keys)
        public static bool KeyDown(char key)
        { return KeyDown((Keys)((int)(char.ToUpper(key)))); }

        // Accepts a character to cast as an Input.Keys object
        // Returns the result of passing the object to KeyUp(Keys)
        public static bool KeyUp(char key)
        { return KeyUp((Keys)((int)(char.ToUpper(key)))); }

        // Accepts a character to cast as an Input.Keys object
        // Returns the result of passing the object to KeyPress(Keys)
        public static bool KeyPress(char key)
        { return KeyPress((Keys)((int)(char.ToUpper(key)))); }

        // Accepts a character to cast as an Input.Keys object
        // Returns the result of passing the object to KeyRelease(Keys)
        public static bool KeyRelease(char key)
        { return KeyRelease((Keys)((int)(char.ToUpper(key)))); }
        #endregion

        #region Gamepad

        // Accepts an Input.Buttons object to check
        // Returns true if button is currently pressed, false if released
        public static bool ButtonDown(Buttons button)
        { return CurrentGamePadState.IsButtonDown(button);}

        // Accepts an Input.Buttons object to check
        // Returns true if button is currently released, false if pressed
        public static bool ButtonUp(Buttons button)
        { return CurrentGamePadState.IsButtonUp(button);}

        // Accepts an Input.Buttons object to check
        // Returns true if button has just been pressed, false otherwise
        public static bool ButtonPress(Buttons button)
        { return CurrentGamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);}

        // Accepts an int to cast as an Input.Buttons object
        // Returns true if button has just been released, false otherwise
        public static bool ButtonRelease(Buttons button)
        { return CurrentGamePadState.IsButtonUp(button) && PreviousGamePadState.IsButtonDown(button); }

        // Accepts an int to cast as an Input.Buttons object
        // Returns the result of passing the object to ButtonDown(Buttons)
        public static bool ButtonDown(int button)
        { return ButtonDown((Buttons)button); }

        // Accepts an int to cast as an Input.Buttons object
        // Returns the result of passing the object to ButtonUp(Buttons)
        public static bool ButtonUp(int button)
        { return ButtonUp((Buttons)button); }

        // Accepts an int to cast as an Input.Buttons object
        // Returns the result of passing the object to ButtonPress(Buttons)
        public static bool ButtonPress(int button)
        { return ButtonPress((Buttons)button); }

        // Accepts an Input.Buttons object to check
        // Returns the result of passing the object to ButtonRelease(Buttons)
        public static bool ButtonRelease(int button)
        { return ButtonRelease((Buttons)button); }
        #endregion

        #region Mouse & Cursor
        // Returns the current position of the mouse cursor on the X axis
        public static int MouseX()
        { return CurrentMouseState.X; }

        // Returns the current position of the mouse cursor on the Y axis
        public static int MouseY()
        { return CurrentMouseState.Y; }

        // Returns the current position of the mouse cursor as a Vector2(X,Y)
        public static Vector2 MousePos()
        { return new Vector2(MouseX(), MouseY()); }

        // Accepts a MouseButton to check
        // Returns true if the button is being pressed
        private static bool MouseButtonCurrentlyDown(MouseButtons button)
        {
            switch(button)
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

        // Accepts a MouseButton to check
        // Returns true if the button was being pressed in the previous frame
        private static bool MouseButtonPreviouslyDown(MouseButtons button)
        {
            switch(button)
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

        // Accepts a MouseButton to check
        // Returns true if the button is being pressed
        public static bool MouseButtonDown(MouseButtons button)
        { return MouseButtonCurrentlyDown(button); }

        // Accepts a MouseButton to check
        // Returns true if the button is not being pressed
        public static bool MouseButtonUp(MouseButtons button)
        { return !MouseButtonCurrentlyDown(button); }

        // Accepts a MouseButton to check
        // Returns true if the button has just been pressed, false otherwise
        public static bool MouseButtonPress(MouseButtons button)
        { return MouseButtonCurrentlyDown(button) && !MouseButtonPreviouslyDown(button); }

        // Accepts a MouseButton to check
        // Returns true if the button has just been released, false otherwise
        public static bool MouseButtonRelease(MouseButtons button)
        { return !MouseButtonCurrentlyDown(button) && MouseButtonPreviouslyDown(button); }

        // Accepts a string to cast as a MouseButton object
        // Returns the result of passing the object to MouseButtonDown(MouseButton)
        public static bool MouseButtonDown(string button)
        { return MouseButtonDown((MouseButtons)Enum.Parse(typeof(MouseButtons), button)); }

        // Accepts a string to cast as a MouseButton object
        // Returns the result of passing the object to MouseButtonUp(MouseButton)
        public static bool MouseButtonUp(string button)
        { return MouseButtonUp((MouseButtons)Enum.Parse(typeof(MouseButtons), button)); }

        // Accepts a string to cast as a MouseButton object
        // Returns the result of passing the object to MouseButtonPress(MouseButton)
        public static bool MouseButtonPress(string button)
        { return MouseButtonPress((MouseButtons)Enum.Parse(typeof(MouseButtons), button)); }

        // Accepts a string to cast as a MouseButton object
        // Returns the result of passing the object to MouseButtonRelease(MouseButton)
        public static bool MouseButtonRelease(string button)
        { return MouseButtonRelease((MouseButtons)Enum.Parse(typeof(MouseButtons), button)); }
        #endregion
        #endregion

        public static void Update()
        {
            #region Debug strings
            if (KeyPress('I'))
            {
                Console.WriteLine("X-axis:" + getX() + " Y-axis:" + getY());
                Console.WriteLine("Raw:" + getRawInput().ToString());
                Console.WriteLine("Normalized:" + getInput().ToString());
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
                if (inputMode == Mode.KBAM)
                {
                    inputMode = Mode.Pad;
                    Console.WriteLine("GAMEPAD CONTROL");
                }
                else
                {
                    inputMode = Mode.KBAM;
                    Console.WriteLine("KEYBOARD CONTROL");
                }
            }

            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
            previousGamePadState = currentGamePadState;

            //polling
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentGamePadState = GamePad.GetState(0);
        }
    }
}
