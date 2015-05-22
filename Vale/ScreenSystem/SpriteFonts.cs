using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem
{
    public class SpriteFonts
    {
        public SpriteFont DetailsFont;
        public SpriteFont FrameRateCounterFont;
        public SpriteFont MenuSpriteFont;

        public SpriteFonts(ContentManager contentManager)
        {
            this.MenuSpriteFont = contentManager.Load<SpriteFont>("Fonts/menuFont");
            this.FrameRateCounterFont = contentManager.Load<SpriteFont>("Fonts/frameRateCounterFont");
            this.DetailsFont = contentManager.Load<SpriteFont>("Fonts/detailsFont");
        }
    }
}
