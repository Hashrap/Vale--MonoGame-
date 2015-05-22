using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem
{
    public enum EntryType
    {
        Screen,
        Separator,
        ExitItem,
        BackItem,
    }

    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display
    /// menu entries in different ways. This also provides an event that will be
    /// raised when the menu entry is selected.
    /// </summary>
    public sealed class MenuEntry
    {
        private Vector2 baseOrigin;
        private float height;
        private MenuScreen menu;
        private float scale;
        private float selectionFade;
        private EntryType type;
        private float width;

        public MenuEntry(MenuScreen menu, string text, EntryType type, GameScreen screen)
        {
            this.Text = text;
            this.Screen = screen;
            this.type = type;
            this.menu = menu;
            this.scale = 0.9f;
            this.Alpha = 1.0f;
        }

        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public float Alpha { get; set; }
        public GameScreen Screen { get; private set; }

        public void Initialize()
        {
            SpriteFont font = this.menu.ScreenManager.Fonts.MenuSpriteFont;
            this.baseOrigin = new Vector2(font.MeasureString(Text).X, font.MeasureString("M").Y) * 0.5f;
            this.width = font.MeasureString(Text).X * 0.8f;
            this.height = font.MeasureString("M").Y * 0.8f;
        }

        public bool IsExitItem()
        {
            return this.type == EntryType.ExitItem;
        }

        public bool IsBackItem()
        {
            return this.type == EntryType.BackItem;
        }

        public bool IsSelectable()
        {
            return this.type != EntryType.Separator;
        }

        public void Update(bool isSelected, GameTime gameTime)
        {
            if (this.type != EntryType.Separator)
            {
                float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
                if (isSelected)
                {
                    this.selectionFade = Math.Min(this.selectionFade + fadeSpeed, 1f);
                }
                else
                {
                    this.selectionFade = Math.Max(this.selectionFade - fadeSpeed, 0f);
                }

                this.scale = 0.7f + 0.1f * this.selectionFade;
            }
        }

        public void Draw()
        {
            SpriteFont font = this.menu.ScreenManager.Fonts.MenuSpriteFont;
            SpriteBatch batch = this.menu.ScreenManager.SpriteBatch;

            Color color = this.type == EntryType.Separator ? Color.DarkOrange : Color.Lerp(Color.White, new Color(255, 210, 0), this.selectionFade);
            color *= Alpha;

            batch.DrawString(font, Text, Position - this.baseOrigin * this.scale + Vector2.One, Color.DarkSlateGray * Alpha * Alpha, 0, Vector2.Zero, this.scale, SpriteEffects.None, 0);
            batch.DrawString(font, Text, Position - this.baseOrigin * this.scale, color, 0, Vector2.Zero, this.scale, SpriteEffects.None, 0);
        }

        public int GetHeight()
        {
            return (int)this.height;
        }

        public int GetWidth()
        {
            return (int)this.width;
        }
    }
}
