using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects
{
    /// <summary>
    /// An object which provides the position of the mouse-pointer in screen-
    /// coordinates. This could be extended to be a "pointer provider" which can
    /// be updated by either the mouse or by a gamepad-controlled cursor.
    /// </summary>
    public class MouseProvider : IUpdate
    {
        public enum Button {
            LMB,
            RMB,
            MMB,
        };

        /// <summary>
        /// The position of the pointer in world-space coordinates.
        /// </summary>
        /// <value>The pointer position in world-space coordinates.</value>
        public Vector2 PointerPosition { get; private set; }

        private GameplayScreen game;

        public MouseProvider(GameplayScreen game)
        {
            this.game = game;
        }

        public bool ButtonPress(Button button)
        {
            Vale.Control.Input.MouseButtons which_button;
            switch (button)
            {
                case Button.LMB:
                    which_button = Vale.Control.Input.MouseButtons.Left;
                    break;
                case Button.RMB:
                    which_button = Vale.Control.Input.MouseButtons.Right;
                    break;
                case Button.MMB:
                    which_button = Vale.Control.Input.MouseButtons.Middle;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return Control.Input.Instance.MouseButtonPress(which_button);
        }

        /// <summary>
        /// Update the pointer position.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            PointerPosition = game.camera.ScreenToWorldCoords(Control.Input.Instance.MousePosition);
        }
    }
}
