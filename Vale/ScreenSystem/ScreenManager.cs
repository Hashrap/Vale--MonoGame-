using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem
{
    public class ScreenManager : DrawableGameComponent
    {
        private InputHelper input;
        private bool isInitialized;
        private List<GameScreen> screens;
        private List<GameScreen> screensToUpdate;
        private List<RenderTarget2D> transitions;

        public ScreenManager(Game game)
            : base(game)
        {
            Content = game.Content;
            Content.RootDirectory = "Content";
            this.input = new InputHelper(this);
            this.screens = new List<GameScreen>();
            this.screensToUpdate = new List<GameScreen>();
            this.transitions = new List<RenderTarget2D>();
        }

        public SpriteBatch SpriteBatch { get; private set; }
        //public LineBatch LineBatch { get; private set; }
        public ContentManager Content { get; private set; }
        public SpriteFonts Fonts { get; private set; }
        //public AssetCreator Assets { get; private set; }

        public override void Initialize()
        {
            this.Fonts = new SpriteFonts(Content);
            base.Initialize();
            this.isInitialized = true;
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);
            //this.LineBatch = new LineBatch(GraphicsDevice);
            //this.Assets = new AssetCreator(GraphicsDevice);
            //this.Assets.LoadContent(Content);
            this.input.LoadContent();

            foreach (GameScreen screen in this.screens)
            {
                screen.LoadContent();
            }

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            foreach (GameScreen screen in this.screens)
            {
                screen.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.input.Update(gameTime);
            this.screensToUpdate.Clear();
            foreach (GameScreen screen in this.screens)
            {
                this.screensToUpdate.Add(screen);
            }
            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (this.screensToUpdate.Count > 0)
            {
                GameScreen screen = this.screensToUpdate[this.screensToUpdate.Count - 1];
                this.screensToUpdate.RemoveAt(this.screensToUpdate.Count - 1);
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across, give
                    // it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        this.input.ShowCursor = screen.HasCursor;
                        this.input.EnableVirtualStick = screen.HasVirtualStick;
                        screen.HandleInput(this.input, gameTime);
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                    {
                        coveredByOtherScreen = true;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int transitionCount = 0;
            foreach (GameScreen screen in this.screens)
            {
                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.TransitionOff)
                {
                    ++transitionCount;
                    if (this.transitions.Count < transitionCount)
                    {
                        PresentationParameters pp = GraphicsDevice.PresentationParameters;
                        this.transitions.Add(
                            new RenderTarget2D(
                                graphicsDevice: GraphicsDevice,
                                width: pp.BackBufferWidth,
                                height: pp.BackBufferHeight,
                                mipMap: false,
                                preferredFormat: SurfaceFormat.Color,
                                preferredDepthFormat: pp.DepthStencilFormat,
                                preferredMultiSampleCount: pp.MultiSampleCount,
                                usage: RenderTargetUsage.DiscardContents));
                    }
                    GraphicsDevice.SetRenderTarget(this.transitions[transitionCount - 1]);
                    GraphicsDevice.Clear(Color.Transparent);
                    screen.Draw(gameTime);
                    GraphicsDevice.SetRenderTarget(null);
                }
            }

            GraphicsDevice.Clear(Color.Black);

            transitionCount = 0;
            foreach (GameScreen screen in this.screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                {
                    continue;
                }

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.TransitionOff)
                {
                    SpriteBatch.Begin(0, BlendState.AlphaBlend);
                    SpriteBatch.Draw(this.transitions[transitionCount], Vector2.Zero, Color.White * screen.TransitionAlpha);
                    SpriteBatch.End();

                    ++transitionCount;
                }
                else
                {
                    screen.Draw(gameTime);
                }
            }
            this.input.Draw();
        }

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            if (this.isInitialized)
            {
                screen.LoadContent();
            }

            this.screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            if (isInitialized)
            {
                screen.UnloadContent();
            }

            this.screens.Remove(screen);
            this.screensToUpdate.Remove(screen);
        }

        public void ClearScreens(GameScreen screenToKeep)
        {

        }
    }
}
