using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.DrawingSystem;
using Vale.ScreenSystem;

namespace Vale.Components
{
    public class Npc
    {
        private float speed = 1.0f;
        private Vector2 position;
        private Rectangle boundingBox;
        private Rectangle eventBox;

        private GameplayScreen gameScreen;

        public Npc(GameplayScreen gameScreen)
        {
            this.gameScreen = gameScreen;
            this.NextPosition = Vector2.Zero;
        }

        public Rectangle BoundingBox
        {
            get { return this.boundingBox; }
            protected set
            {
                this.boundingBox = value;
                this.eventBox.X = this.boundingBox.X - this.boundingBox.Width / 2;
                this.eventBox.Y = this.boundingBox.Y - this.boundingBox.Height / 2;
            }
        }

        public Vector2 Position {
            get { return this.position; }
            set
            {
                this.position = value;
                this.boundingBox.X = (int)(value.X + .5f);
                this.boundingBox.Y = (int)(value.Y + .5f);
                this.eventBox.X = (int)(this.position.X - this.boundingBox.Width / 2);
                this.eventBox.Y = (int)(this.position.Y - this.boundingBox.Height / 2);
            }
        }
        public Vector2 Velocity { get; set; }
        public Vector2 NextPosition { get; set; }

        public Rectangle EventBox
        {
            get { return this.eventBox; }
            set { this.eventBox = value; }
        }

        public AnimatedSprite Sprite { get; set; }

        public virtual void LoadContent(ContentManager content, string character)
        {
            // Move the NPC code into a Character
            Dictionary<AnimationKey, Animation> animation = new Dictionary<AnimationKey, Animation>();
            animation[AnimationKey.North] =
                new Animation(3, 16, 16, 0, 16 * 0);
            animation[AnimationKey.NorthEast] =
                new Animation(3, 16, 16, 0, 16 * 1);
            animation[AnimationKey.East] =
                new Animation(3, 16, 16, 0, 16 * 2);
            animation[AnimationKey.SouthEast] =
                new Animation(3, 16, 16, 0, 16 * 3);
            animation[AnimationKey.South] =
                new Animation(3, 16, 16, 0, 16 * 4);
            animation[AnimationKey.SouthWest] =
                new Animation(3, 16, 16, 0, 16 * 5);
            animation[AnimationKey.West] =
                new Animation(3, 16, 16, 0, 16 * 6);
            animation[AnimationKey.NorthWest] =
                new Animation(3, 16, 16, 0, 16 * 7);

            this.Sprite = new AnimatedSprite(content.Load<Texture2D>("NpcSprites/" + character), animation);
            this.BoundingBox = new Rectangle(0, 0, this.Sprite.Width, this.Sprite.Height);
            this.Sprite.CurrentAnimation = AnimationKey.North;
            this.Sprite.IsAnimating = true;

            this.EventBox = new Rectangle((int)(this.BoundingBox.X - this.BoundingBox.Width / 2f), (int)(this.BoundingBox.Y - this.BoundingBox.Height / 2f), (int)(this.BoundingBox.Width * 2f), (int)(this.BoundingBox.Height * 2f));
        }

        public virtual void ProcessIntent()
        {
            // TODO: Process "input" from the AI
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.NextPosition == Vector2.Zero)
            {
                Vector2 position = this.Position;
                position += this.Velocity;
                this.Position =
                    new Vector2(
                        MathHelper.Clamp(position.X, 0, this.gameScreen.GameMap.WorldWidth - this.BoundingBox.Width),
                        MathHelper.Clamp(position.Y, 0, this.gameScreen.GameMap.WorldHeight - this.BoundingBox.Height)
                        );
            }
            else
            {
                this.Position = this.NextPosition;
                this.NextPosition = Vector2.Zero;
            }
        }

        public virtual void TriggerEvent()
        {
            System.Diagnostics.Debug.WriteLine("EVENT!");
        }
    }
}