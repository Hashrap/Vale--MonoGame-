using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem
{
    public class MenuScreen : GameScreen
    {
        private const float NumEntries = 15;
        private List<MenuEntry> menuEntries = new List<MenuEntry>();
        private string menuTitle;
        private Vector2 titlePosition;
        private Vector2 titleOrigin;
        private int selectedEntry;
        private float menuBorderTop;
        private float menuBorderBottom;
        private float menuBorderMargin;
        private float menuOffset;
        private float maxOffset;
        private Texture2D scrollButtonTexture;
        private Texture2D sliderTexture;
        private MenuButton scrollUp;
        private MenuButton scrollDown;
        private MenuButton scrollSlider;
        private bool scrollLock;

        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;
            this.TransitionOnTime = TimeSpan.FromSeconds(0.7);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.7);
            this.HasCursor = true;
        }

        public void AddMenuItem(string name, EntryType type, GameScreen screen)
        {
            MenuEntry entry = new MenuEntry(this, name, type, screen);
            this.menuEntries.Add(entry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Fonts.MenuSpriteFont;
            this.scrollButtonTexture = ScreenManager.Content.Load<Texture2D>("Common/arrow");
            this.sliderTexture = ScreenManager.Content.Load<Texture2D>("Common/slider");

            float scrollBarPos = viewport.Width / 2f;
            for (int i = 0; i < this.menuEntries.Count; i++)
            {
                this.menuEntries[i].Initialize();
                scrollBarPos = Math.Min(scrollBarPos, (viewport.Width - this.menuEntries[i].GetWidth()) / 2f);
            }
            scrollBarPos -= this.scrollButtonTexture.Width + 2f;
            this.titleOrigin = font.MeasureString(this.menuTitle) / 2f;
            this.titlePosition = new Vector2(viewport.Width / 2f, font.MeasureString("M").Y / 2f + 10f);

            this.menuBorderMargin = font.MeasureString("M").Y * 0.8f;
            this.menuBorderTop = (viewport.Height - this.menuBorderMargin * (NumEntries - 1)) / 2f;
            this.menuBorderBottom = (viewport.Height + this.menuBorderMargin * (NumEntries - 1)) / 2f;

            this.menuOffset = 0f;
            this.maxOffset = Math.Max(0f, (this.menuEntries.Count - NumEntries) * this.menuBorderMargin);

            this.scrollUp = new MenuButton(this.scrollButtonTexture, false, new Vector2(scrollBarPos, this.menuBorderTop - this.scrollButtonTexture.Height), this);
            this.scrollDown = new MenuButton(this.scrollButtonTexture, true, new Vector2(scrollBarPos, this.menuBorderBottom + this.scrollButtonTexture.Height), this);
            this.scrollSlider = new MenuButton(this.sliderTexture, false, new Vector2(scrollBarPos, this.menuBorderTop), this);
            this.scrollLock = false;
        }

        /// <summary>
        /// Returns the index of the menu entry at the given mouse position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Index of menu entry if valid, -1 otherwise</returns>
        private int GetMenuEntryAt(Vector2 position)
        {
            int index = 0;
            foreach (MenuEntry entry in this.menuEntries)
            {
                float width = entry.GetWidth();
                float height = entry.GetHeight();
                Rectangle rect = new Rectangle((int)(entry.Position.X - width / 2f), (int)(entry.Position.Y - height / 2f), (int)width, (int)height);
                if (rect.Contains((int)position.X, (int)position.Y) && entry.Alpha > 0.1f)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting or
        /// cancelling the menu.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="gameTime"></param>
        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            int hoverIndex = GetMenuEntryAt(input.Cursor);
            if (hoverIndex > -1 && this.menuEntries[hoverIndex].IsSelectable() && !this.scrollLock)
            {
                this.selectedEntry = hoverIndex;
            }
            else
            {
                this.selectedEntry = -1;
            }

            this.scrollSlider.Hover = false;
            if (input.IsCursorValid)
            {
                this.scrollUp.Collide(input.Cursor);
                this.scrollDown.Collide(input.Cursor);
                this.scrollSlider.Collide(input.Cursor);
            }
            else
            {
                this.scrollUp.Hover = false;
                this.scrollDown.Hover = false;
                this.scrollLock = false;
            }

            // Accept or cancel the menu?
            if (input.IsMenuSelect() && this.selectedEntry != -1)
            {
                if (this.menuEntries[this.selectedEntry].IsExitItem())
                {
                    ScreenManager.Game.Exit();
                }
                else if (this.menuEntries[this.selectedEntry].IsBackItem())
                {
                    this.ExitScreen();
                }
                else if (this.menuEntries[this.selectedEntry].Screen != null)
                {
                    ScreenManager.AddScreen(this.menuEntries[this.selectedEntry].Screen);
                }
            }
            else if (input.IsMenuCancel())
            {
                ScreenManager.Game.Exit();
            }

            if (input.IsMenuPressed())
            {
                if (this.scrollUp.Hover)
                {
                    this.menuOffset = Math.Max(this.menuOffset - 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f);
                    this.scrollLock = false;
                }

                if (this.scrollDown.Hover)
                {
                    this.menuOffset = Math.Min(this.menuOffset + 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, this.maxOffset);
                    this.scrollLock = false;
                }

                if (this.scrollSlider.Hover)
                {
                    this.scrollLock = true;
                }
            }

            if (input.IsMenuReleased())
            {
                this.scrollLock = false;
            }

            if (this.scrollLock)
            {
                this.scrollSlider.Hover = true;
                this.menuOffset = Math.Max(Math.Min(((input.Cursor.Y - this.menuBorderTop) / (this.menuBorderBottom - this.menuBorderTop)) * this.maxOffset, this.maxOffset), 0f);
            }
        }

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By
        /// default all menu entries are lined up in a vertical list, centered
        /// on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions using a power-
            // curve to make it more interesting.
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = Vector2.Zero;
            position.Y = this.menuBorderTop - this.menuOffset;
            for (int i = 0; i < this.menuEntries.Count; i++)
            {
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2f;
                if (ScreenState == ScreenState.TransitionOn)
                {
                    position.X -= transitionOffset * 256;
                }
                else
                {
                    position.X += transitionOffset * 256;
                }

                this.menuEntries[i].Position = position;
                if (position.Y < this.menuBorderTop)
                {
                    this.menuEntries[i].Alpha = 1f - Math.Min(this.menuBorderTop - position.Y, this.menuBorderMargin) / this.menuBorderMargin;
                }
                else if (position.Y > this.menuBorderBottom)
                {
                    this.menuEntries[i].Alpha = 1f - Math.Min(position.Y - this.menuBorderBottom, this.menuBorderMargin) / this.menuBorderMargin;
                }
                else
                {
                    this.menuEntries[i].Alpha = 1f;
                }

                // Move down for the next entry by the size of this entry
                position.Y += this.menuEntries[i].GetHeight();
            }

            Vector2 scrollPos = this.scrollSlider.Position;
            scrollPos.Y = MathHelper.Lerp(this.menuBorderTop, this.menuBorderBottom, this.menuOffset / this.maxOffset);
            this.scrollSlider.Position = scrollPos;
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < this.menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == this.selectedEntry);
                this.menuEntries[i].Update(isSelected, gameTime);
            }

            this.scrollUp.Update(gameTime);
            this.scrollDown.Update(gameTime);
            this.scrollSlider.Update(gameTime);
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Fonts.MenuSpriteFont;

            spriteBatch.Begin();
            foreach (var entry in this.menuEntries)
            {
                entry.Draw();
            }

            // Power curve sliding transition
            Vector2 transitionOffset = new Vector2(0f, (float)Math.Pow(TransitionPosition, 2) * 100f);

            spriteBatch.DrawString(
                spriteFont: font,
                text: this.menuTitle,
                position: this.titlePosition - transitionOffset + Vector2.One * 2f,
                color: Color.Black,
                rotation: 0,
                origin: this.titleOrigin,
                scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0);
            spriteBatch.DrawString(
                spriteFont: font,
                text: this.menuTitle,
                position: this.titlePosition - transitionOffset,
                color: new Color(255, 210, 0),
                rotation: 0,
                origin: this.titleOrigin,
                scale: 1f,
                effects: SpriteEffects.None,
                layerDepth: 0);
            //this.scrollUp.Draw();
            //this.scrollSlider.Draw();
            //this.scrollDown.Draw();
            spriteBatch.End();
        }
    }
}
