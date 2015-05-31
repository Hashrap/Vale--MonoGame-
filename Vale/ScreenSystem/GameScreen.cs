using Microsoft.Xna.Framework;
using System;

namespace Vale.ScreenSystem
{
    public abstract class GameScreen
    {
        public ScreenManager ScreenManager
        {
            get;
            set;
        }

        public abstract void LoadContent();
        public abstract void UnloadContent();

        public virtual void Update(GameTime gameTime)
        {
            // TODO: Manage screen state (handle the coupling with the ScreenManager)
        }

        public virtual void Draw(GameTime gameTime) { }

        public void Exit()
        {
            // TODO: Add transitions
            ScreenManager.Remove(this);
        }

        // TODO: It could be potentially beneficial to pass in an input state
        // and give only the top screen access to the input. This allows a menu
        // system with pop-ups to exist without input affecting screens lower in
        // the screen manager stack.
        // public abstract void HandleInput(InputState);
    }
}
