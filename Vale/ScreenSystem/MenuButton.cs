using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display
    /// menu entries in different ways. This also provides an event that will be
    /// raised when the menu entry is selected.
    /// </summary>
    public sealed class MenuButton
    {
        private Vector2 baseOrigin;
        private bool flip;
        private float scale;
        private GameScreen screen;
        private float selectionFade;
        private Texture2D sprite;

        public MenuButton(Texture2D sprite, bool flip, Vector2 position, GameScreen screen)
        {
            this.screen = screen;
            this.scale = 1f;
            this.sprite = sprite;
            this.baseOrigin = new Vector2(this.sprite.Width / 2f, this.sprite.Height / 2f);
            this.Hover = false;
            this.flip = flip;
            this.Position = position;
        }

        public Vector2 Position { get; set; }
        public bool Hover { get; set; }

        public void Update(GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            this.selectionFade = Hover ? Math.Min(this.selectionFade + fadeSpeed, 1f) : Math.Max(this.selectionFade - fadeSpeed, 0f);
            this.scale = 1f + 0.1f * this.selectionFade;
        }

        public void Collide(Vector2 position)
        {
            Rectangle collisionBox = new Rectangle((int)(Position.X - this.sprite.Width / 2f), (int)(Position.Y - this.sprite.Height / 2f), (this.sprite.Width),this.sprite.Height);
            Hover = collisionBox.Contains((int)position.X, (int)position.Y);
        }

        public void Draw()
        {
            SpriteBatch batch = this.screen.ScreenManager.SpriteBatch;
            Color color = Color.Lerp(Color.White, new Color(255, 210, 0), this.selectionFade);
            batch.Draw(this.sprite, Position - this.baseOrigin * this.scale, null, color, 0f, Vector2.Zero, this.scale, this.flip ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }
}
