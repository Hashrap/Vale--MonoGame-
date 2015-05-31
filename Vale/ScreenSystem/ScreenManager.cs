using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Vale.ScreenSystem
{
    public class ScreenManager : DrawableGameComponent
    {
        // Holds references to all of the 
        List<GameScreen> screenStack = new List<GameScreen>();
        // The queue is the stack reversed and is what is iterated over during a call to Update()
        List<GameScreen> screenQueue = new List<GameScreen>();

        // Initialization happens independent of creation. The Content Manager won't be loaded until after initialization
        bool isInitialized = false;

        // Keep a SpriteBatch so that GameScreens need not instantiate their own
        public SpriteBatch SpriteBatch
        {
            get;
            set;
        }

        public ScreenManager(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (var screen in screenStack)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            foreach (var screen in screenStack)
            {
                screen.UnloadContent();
            }
        }

        public void Add(GameScreen screen)
        {
            screen.ScreenManager = this;
            if (isInitialized)
            {
                screen.LoadContent();
            }
            screenStack.Add(screen);
        }

        public void Remove(GameScreen screen)
        {
            if (isInitialized)
            {
                screen.UnloadContent();
            }
            screenStack.Remove(screen);
            screenQueue.Remove(screen);
        }

        public override void Update(GameTime gameTime)
        {
            screenQueue.Clear();
            foreach (var screen in screenStack)
            {
                screenQueue.Add(screen);
            }

            while (screenQueue.Count > 0)
            {
                GameScreen screen = screenQueue[screenQueue.Count - 1];
                screenQueue.RemoveAt(screenQueue.Count - 1);

                // Update the screen and if it's at the top and active, then allow it to handle input
                screen.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var screen in screenStack)
            {
                // TODO: If the screen is not hidden
                screen.Draw(gameTime);
            }
        }
    }
}