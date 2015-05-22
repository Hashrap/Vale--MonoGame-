using System;
using Microsoft.Xna.Framework;

namespace Vale.DrawingSystem
{
    public enum AnimationKey
    {
        South,
        South_SouthEast, SouthEast, East_SouthEast, East,
        East_NorthEast, NorthEast, North_NorthEast, North,
        North_NorthWest, NorthWest, West_NorthWest, West,
        West_SouthWest, SouthWest, South_SouthWest
    }

    public class Animation : ICloneable
    {
        Rectangle[] frames;
        int framesPerSecond;
        TimeSpan frameLength;
        TimeSpan frameTimer;
        int currentFrame;

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            this.frames = new Rectangle[frameCount];
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                    xOffset + (frameWidth * i),
                    yOffset,
                    frameWidth,
                    frameHeight);
            }
            this.FramesPerSecond = 8;
            this.Reset();
        }

        private Animation(Animation animation)
        {
            this.frames = animation.frames;
            this.FramesPerSecond = 8;
        }

        public int FramesPerSecond
        {
            get { return this.framesPerSecond; }
            set
            {
                if (value < 1)
                {
                    this.framesPerSecond = 1;
                }
                else if (value > 60)
                {
                    framesPerSecond = 60;
                }
                else
                {
                    framesPerSecond = value;
                }
                this.frameLength = TimeSpan.FromSeconds(1 / (double)framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRectangle
        {
            get { return this.frames[this.currentFrame]; }
        }

        public int CurrentFrame
        {
            get { return this.currentFrame; }
            set
            {
                this.currentFrame = (int)MathHelper.Clamp(value, 0, frames.Length - 1);
            }
        }

        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        public void Update(GameTime gameTime)
        {
            this.frameTimer += gameTime.ElapsedGameTime;
            if (frameTimer >= frameLength)
            {
                frameTimer = TimeSpan.Zero;
                this.CurrentFrame = (CurrentFrame + 1) % frames.Length;
            }
        }

        public void Reset()
        {
            this.CurrentFrame = 0;
            this.frameTimer = TimeSpan.Zero;
        }

        public object Clone()
        {
            Animation animationClone = new Animation(this);
            animationClone.FrameWidth = this.FrameWidth;
            animationClone.FrameHeight = this.FrameHeight;
            animationClone.Reset();
            return animationClone;
        }
    }
}
