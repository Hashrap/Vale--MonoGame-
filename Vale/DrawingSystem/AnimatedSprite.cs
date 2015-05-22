using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.DrawingSystem
{
    public class AnimatedSprite
    {
        public Dictionary<AnimationKey, Animation> animations { get; set; }

        Texture2D texture;

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            this.texture = sprite;
            this.animations = new Dictionary<AnimationKey, Animation>();
            foreach (AnimationKey key in animation.Keys)
            {
                animations.Add(key, (Animation)animation[key].Clone());
            }
            this.IsAnimating = true;
        }

        public AnimationKey CurrentAnimation { get; set; }
        public bool IsAnimating { get; set; }
        public int Width { get { return animations[CurrentAnimation].FrameWidth; } }
        public int Height { get { return animations[CurrentAnimation].FrameHeight; } }
        public Vector2 Position { get; set; }
       

        public void Update(GameTime gameTime)
        {
            if (this.IsAnimating)
            {
                animations[CurrentAnimation].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(
                texture,
                Position,
                animations[CurrentAnimation].CurrentFrameRectangle,
                Color.White);
        }
    }
}
