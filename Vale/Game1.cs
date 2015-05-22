using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vale.ScreenSystem;

namespace Vale
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
		private const int SCREEN_WIDTH = 1280;
		private const int SCREEN_HEIGHT = 720;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		private float screenCounter = 0;
		private bool doneScreenCounting = false;

		public ScreenManager ScreenManager { get; set;}

		public Game1()
        {
			Window.Title = "Vale";
            graphics = new GraphicsDeviceManager(this);
			graphics.PreferMultiSampling = true;
			graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
			graphics.IsFullScreen = false;

			IsMouseVisible = true;
			IsFixedTimeStep = true;

            Content.RootDirectory = "Content";
			ScreenManager = new ScreenManager (this);
			Components.Add (ScreenManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Input.Initialize(Input.Mode.KBAM);
//            player = new Player();
//            player.Initialize(Content.Load<Texture2D>("Art\\bksq20x20"));
			MenuScreen newGameMenu = new MenuScreen("New Game");
			GameplayScreen slot1 = new GameplayScreen("slot1");
			newGameMenu.AddMenuItem("Slot 1", EntryType.Screen, slot1);
			newGameMenu.AddMenuItem("", EntryType.Separator, null);
			newGameMenu.AddMenuItem("Back", EntryType.BackItem, null);

			MenuScreen loadGameMenu = new MenuScreen("Load Game");
			bool empty = true;
			if (slot1.SaveExists)
			{
				empty = false;
				loadGameMenu.AddMenuItem("Slot 1", EntryType.Screen, slot1);
			}
			if (empty)
			{
				loadGameMenu.AddMenuItem("No saves to load.", EntryType.Separator, null);
			}

			loadGameMenu.AddMenuItem("", EntryType.Separator, null);
			loadGameMenu.AddMenuItem("Back", EntryType.BackItem, null);

			MenuScreen mainMenu = new MenuScreen("Vale");
			mainMenu.AddMenuItem("New Game", EntryType.Screen, newGameMenu);
			mainMenu.AddMenuItem("Load Game", EntryType.Screen, loadGameMenu);
			mainMenu.AddMenuItem("", EntryType.Separator, null);
			mainMenu.AddMenuItem("Quit", EntryType.ExitItem, null);

			ScreenManager.AddScreen(new BackgroundScreen());
			ScreenManager.AddScreen(mainMenu);
			ScreenManager.AddScreen(new SplashScreen(TimeSpan.FromSeconds(3.0)));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
			
			if (!doneScreenCounting)
			{
				screenCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (screenCounter < 2)
				{
					this.graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
					this.graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
					this.graphics.ApplyChanges();
				}
			}

            // TODO: Add your update logic here
//            player.Update(gameTime);
//            Input.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

//            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
