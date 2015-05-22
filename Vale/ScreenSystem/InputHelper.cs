using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vale.DrawingSystem;
namespace Vale.ScreenSystem
{
    public enum MouseButtons
    {
        LeftButton,
        MiddleButton,
        RightButton,
        ExtraButton1,
        ExtraButton2,
    }

    public class InputHelper
    {
        private bool handleVirtualStick;
        private bool cursorIsVisible;
        private Sprite cursorSprite;
        private ScreenManager manager;
        private Viewport viewport;

        public InputHelper(ScreenManager manager)
        {
            KeyboardState = new KeyboardState();
            GamePadState = new GamePadState();
            MouseState = new MouseState();
            VirtualState = new GamePadState();

            PreviousKeyboardState = new KeyboardState();
            PreviousGamePadState = new GamePadState();
            PreviousMouseState = new MouseState();
            PreviousVirtualState = new GamePadState();

            this.manager = manager;

            cursorIsVisible = false;
            IsCursorMoved = false;
            IsCursorValid = true;
            Cursor = Vector2.Zero;
            handleVirtualStick = false;
        }

        public GamePadState GamePadState { get; private set; }

        public KeyboardState KeyboardState { get; private set; }

        public MouseState MouseState { get; private set; }

        public GamePadState VirtualState { get; private set; }

        public GamePadState PreviousGamePadState { get; private set; }

        public KeyboardState PreviousKeyboardState { get; private set; }

        public MouseState PreviousMouseState { get; private set; }

        public GamePadState PreviousVirtualState { get; private set; }

        public bool ShowCursor
        {
            get { return this.cursorIsVisible && IsCursorValid; }
            set { this.cursorIsVisible = value; }
        }

        public bool EnableVirtualStick
        {
            get { return this.handleVirtualStick; }
            set { this.handleVirtualStick = value; }
        }

        public Vector2 Cursor { get; private set; }

        public bool IsCursorMoved { get; private set; }

        public bool IsCursorValid { get; private set; }

        public void LoadContent()
        {
            this.cursorSprite = new Sprite(this.manager.Content.Load<Texture2D>("Common/cursor"));
            this.viewport = this.manager.GraphicsDevice.Viewport;
        }

        public void Update(GameTime gameTime)
        {
            PreviousKeyboardState = KeyboardState;
            PreviousGamePadState = GamePadState;
            PreviousMouseState = MouseState;

            if (this.handleVirtualStick)
                PreviousVirtualState = VirtualState;

            KeyboardState = Keyboard.GetState();
            GamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState = Mouse.GetState();

            if (this.handleVirtualStick)
            {
#if XBOX
                VirtualState = GamePad.GetState(PlayerIndex.One);
#elif WINDOWS
                VirtualState = GamePad.GetState(PlayerIndex.One).IsConnected ? GamePad.GetState(PlayerIndex.One) : HandleVirtualStickWin();
#endif
            }

            Vector2 oldCursor = Cursor;
            if (GamePadState.IsConnected && GamePadState.ThumbSticks.Left != Vector2.Zero)
            {
                Vector2 temp = GamePadState.ThumbSticks.Left;
                Cursor += temp * new Vector2(300f, -300f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Mouse.SetPosition((int)Cursor.X, (int)Cursor.Y);
            }
            else
            {
                Cursor = new Vector2(MouseState.X, MouseState.Y);
            }

            Cursor = new Vector2(MathHelper.Clamp(Cursor.X, 0f, this.viewport.Width), MathHelper.Clamp(Cursor.Y, 0f, this.viewport.Height));

            IsCursorMoved = (IsCursorValid && oldCursor != Cursor);
#if WINDOWS
            IsCursorValid = this.viewport.Bounds.Contains(MouseState.X, MouseState.Y);
#endif
        }

        public void Draw()
        {
            if (this.cursorIsVisible && IsCursorValid)
            {
                //this.manager.SpriteBatch.Begin();
                //this.manager.SpriteBatch.Draw(this.cursorSprite.Texture, Cursor, null, Color.White, 0f, this.cursorSprite.Origin, 1f, SpriteEffects.None, 0f);
                //this.manager.SpriteBatch.End();
            }
        }

        private GamePadState HandleVirtualStickWin()
        {
            Vector2 leftStick = Vector2.Zero;
            List<Buttons> buttons = new List<Buttons>();

            if (KeyboardState.IsKeyDown(Keys.A))
                leftStick.X -= 1f;
            if (KeyboardState.IsKeyDown(Keys.S))
                leftStick.Y -= 1f;
            if (KeyboardState.IsKeyDown(Keys.D))
                leftStick.X += 1f;
            if (KeyboardState.IsKeyDown(Keys.W))
                leftStick.Y += 1f;
            if (KeyboardState.IsKeyDown(Keys.Space))
                buttons.Add(Buttons.A);
            if (KeyboardState.IsKeyDown(Keys.LeftControl))
                buttons.Add(Buttons.B);
            if (leftStick != Vector2.Zero)
                leftStick.Normalize();

            return new GamePadState(leftStick, Vector2.Zero, 0f, 0f, buttons.ToArray());
        }

        public bool IsNewKeyPress(Keys key)
        {
            return (KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key));
        }

        public bool IsNewKeyRelease(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) && KeyboardState.IsKeyUp(key));
        }

        public bool IsNewButtonPress(Buttons button)
        {
            return (GamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button));
        }

        public bool IsNewButtonRelease(Buttons button)
        {
            return (PreviousGamePadState.IsButtonDown(button) && GamePadState.IsButtonUp(button));
        }

        public bool IsNewMouseButtonPress(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (MouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (MouseState.XButton1 == ButtonState.Pressed && PreviousMouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (MouseState.XButton2 == ButtonState.Pressed && PreviousMouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }

        public bool IsNewMouseButtonRelease(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (MouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed);
                case MouseButtons.RightButton:
                    return (MouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed);
                case MouseButtons.MiddleButton:
                    return (MouseState.MiddleButton == ButtonState.Released && PreviousMouseState.MiddleButton == ButtonState.Pressed);
                case MouseButtons.ExtraButton1:
                    return (MouseState.XButton1 == ButtonState.Released && PreviousMouseState.XButton1 == ButtonState.Pressed);
                case MouseButtons.ExtraButton2:
                    return (MouseState.XButton2 == ButtonState.Released && PreviousMouseState.XButton2 == ButtonState.Pressed);
                default:
                    return false;
            }
        }

        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter) || IsNewButtonPress(Buttons.A) || IsNewButtonPress(Buttons.Start) || IsNewMouseButtonPress(MouseButtons.LeftButton);
        }

        public bool IsMenuPressed()
        {
            return KeyboardState.IsKeyDown(Keys.Space) || KeyboardState.IsKeyDown(Keys.Enter) || GamePadState.IsButtonDown(Buttons.A) || GamePadState.IsButtonDown(Buttons.Start) || MouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsMenuReleased()
        {
            return IsNewKeyRelease(Keys.Space) || IsNewKeyRelease(Keys.Enter) || IsNewButtonRelease(Buttons.A) || IsNewButtonRelease(Buttons.Start) || IsNewMouseButtonRelease(MouseButtons.LeftButton);
        }

        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape) || IsNewButtonPress(Buttons.Back);
        }
    }
}