using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vale
{
	public class ScreenManager : Game
	{
		public static GraphicsDeviceManager Graphics;
		public static SpriteBatch SBatch;
		public static Dictionary<string, Texture2D> Textures2D = new Dictionary<string, Texture2D> ();
		public static Dictionary<string, Texture3D> Textures3D = new Dictionary<string, Texture3D> ();
		public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont> ();
		public static Dictionary<string, Model> Models = new Dictionary<string, Model> ();
		public static List<GameScreen> ScreenList = new List<GameScreen> ();
		public static ContentManager ContentManager;

		public ScreenManager ()
		{
			Graphics = new GraphicsDeviceManager (this);
			Graphics.PreferredBackBufferWidth = 800;
			Graphics.PreferredBackBufferHeight = 600;
			Graphics.IsFullScreen = false;
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			base.Initialize ();
			Textures2D = new Dictionary<string, Texture2D> ();
			Textures3D = new Dictionary<string, Texture3D> ();
			Models = new Dictionary<string, Model> ();
			Fonts = new Dictionary<string, SpriteFont> ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			ContentManager = Content;
			// Create a new SpriteBatch, which can be used to draw textures.
			SBatch = new SpriteBatch (GraphicsDevice);

			// TODO: use this.Content to load your game content here
			AddScreen(new TestScreen());
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent ()
		{
			// TODO: Unload any non ContentManager content here
			foreach (var screen in ScreenList) {
				screen.UnloadAssets ();
			}
			Textures2D.Clear ();
			Textures3D.Clear ();
			Fonts.Clear ();
			Models.Clear ();
			ScreenList.Clear ();
			Content.Unload ();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			try {
				if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
					Exit ();
				
				var startIndex = ScreenList.Count - 1;
				while (ScreenList [startIndex].IsPopup && ScreenList [startIndex].IsActive) {
					startIndex--;
				}
				for (var i = startIndex; i < ScreenList.Count; i++) {
					ScreenList [i].Update (gameTime);
				}
			} catch (Exception ex) {
				// TODO: Log error
				throw ex;
			} finally {
				base.Update (gameTime);
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			var startIndex = ScreenList.Count - 1;
			while (ScreenList [startIndex].IsPopup) {
				startIndex--;
			}

			GraphicsDevice.Clear (ScreenList [startIndex].BackgroundColor);
			Graphics.GraphicsDevice.Clear (ScreenList [startIndex].BackgroundColor);

			for (var i = startIndex; i < ScreenList.Count; i++) {
				ScreenList [i].Draw (gameTime);
			}
			base.Draw (gameTime);

//			// TODO: Add your drawing code here
//			spriteBatch.Begin();
//
//			player.Draw(spriteBatch);
//
//			spriteBatch.End();
//
//			base.Draw(gameTime);
		}

		public static void AddScreen (GameScreen gameScreen)
		{
			ScreenList.Add (gameScreen);
			gameScreen.LoadAssets ();
		}

		public static void RemoveScreen (GameScreen gameScreen)
		{
			gameScreen.UnloadAssets ();
			ScreenList.Remove (gameScreen);
			if (ScreenList.Count == 0) {
				AddScreen (new TestScreen ());
			}
		}

		public static void SwitchScreen(GameScreen currentScreen, GameScreen targetScreen)
		{
			RemoveScreen (currentScreen);
			AddScreen (targetScreen);
		}
	}
}
