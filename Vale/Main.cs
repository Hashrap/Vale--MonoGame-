using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vale.Control;
using Vale.Parsing;
using Vale.ScreenSystem;
using Vale.ScreenSystem.Screens;

namespace Vale
{
    public class Main : Game
    {
        private const int SCREEN_WIDTH = 1280;
        private const int SCREEN_HEIGHT = 720;

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        Input input;

        private string[] preload = {
                                       "Common/cursor",
                                   };

        public Main()
        {
            Window.Title = "Vale";
            Content.RootDirectory = "Content";

            //UnitParser.Instance.ParseUnits();
            JsonParser.Instance.ParseData<UnitInfo>("..\\..\\..\\Content\\Data\\units.txt");
            JsonParser.Instance.ParseData<AbilityInfo>("..\\..\\..\\Content\\Data\\abilities.txt");
            
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            this.IsMouseVisible = false;

            // TODO: Create the screen manager component
            screenManager = new ScreenManager(this);

            input = Input.Instance;
            Components.Add(screenManager);

            // TODO: Create a gameplay screen and add it to the screen manager
            screenManager.Add(new BackgroundScreen());
            screenManager.Add(new SplashScreen(TimeSpan.FromSeconds(1.0)));
        }

        protected override void Initialize()
        {
            base.Initialize();
            input.Initialize(Input.Mode.KeyboardMouse);
        }

        /// <summary>
        /// Loads graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Pre-loaded assets

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Azure);

            // The real drawing happens inside the screen manager component
            // Monogame handles calling it if it's been added to the Component

            base.Draw(gameTime);
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update(gameTime);
#if DEBUG
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();
#endif

            base.Update(gameTime);
        }
    }
}
