using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vale.DrawingSystem;
using Vale.ScreenSystem;

namespace Vale.Components
{
    public class Character
    {
        private float speed = 2f;
        private GameplayScreen gameScreen;

        public Character(GameplayScreen gameScreen)
        {
            this.gameScreen = gameScreen;
        }

        public AnimatedSprite Sprite { get; set; }

        public virtual void LoadContent(ContentManager content, string character)
        {
            Dictionary<AnimationKey, Animation> animation = new Dictionary<AnimationKey, Animation>();
            animation[AnimationKey.South] =
                new Animation(3, 16, 16, 0, 16 * 0);
            animation[AnimationKey.South_SouthEast] =
                new Animation(3, 16, 16, 0, 16 * 1);
            animation[AnimationKey.SouthEast] =
                new Animation(3, 16, 16, 0, 16 * 2);
            animation[AnimationKey.East_SouthEast] =
                new Animation(3, 16, 16, 0, 16 * 3);
            animation[AnimationKey.East] =
                new Animation(3, 16, 16, 0, 16 * 4);
            animation[AnimationKey.East_NorthEast] =
                new Animation(3, 16, 16, 0, 16 * 5);
            animation[AnimationKey.NorthEast] =
                new Animation(3, 16, 16, 0, 16 * 6);
            animation[AnimationKey.North_NorthEast] =
                new Animation(3, 16, 16, 0, 16 * 7);
            animation[AnimationKey.North] =
                new Animation(3, 16, 16, 0, 16 * 8);
            animation[AnimationKey.North_NorthWest] =
                new Animation(3, 16, 16, 0, 16 * 9);
            animation[AnimationKey.NorthWest] =
                new Animation(3, 16, 16, 0, 16 * 10);
            animation[AnimationKey.West_NorthWest] =
                new Animation(3, 16, 16, 0, 16 * 11);
            animation[AnimationKey.West] =
                new Animation(3, 16, 16, 0, 16 * 12);
            animation[AnimationKey.West_SouthWest] =
                new Animation(3, 16, 16, 0, 16 * 13);
            animation[AnimationKey.SouthWest] =
                new Animation(3, 16, 16, 0, 16 * 14);
            animation[AnimationKey.South_SouthWest] =
                new Animation(3, 16, 16, 0, 16 * 15);

            this.Sprite = new AnimatedSprite(content.Load<Texture2D>("PlayerSprites/" + character), animation);

            // TODO: Load player attributes from a file.
        }

        /// <summary>
        /// Gets a unit vector (or zero) indicating the Character's direction of movement
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual Vector2 Movement(InputHelper input)
        {
            // Movement
            Vector2 velocity = Vector2.Zero;
            if (input.KeyboardState.IsKeyDown(Keys.W))
            {
                this.Sprite.IsAnimating = true;
                // Move the character north
                velocity.Y = -1;
            }
            else if (input.KeyboardState.IsKeyDown(Keys.S))
            {
                this.Sprite.IsAnimating = true;
                // Move the character south
                velocity.Y = 1;
            }
            else
            {
                velocity.Y = 0;
            }

            if (input.KeyboardState.IsKeyDown(Keys.A))
            {
                this.Sprite.IsAnimating = true;
                // Move the character West
                velocity.X = -1;
            }
            else if (input.KeyboardState.IsKeyDown(Keys.D))
            {
                this.Sprite.IsAnimating = true;
                // Move the character East
                velocity.X = 1;
            }
            else
            {
                velocity.X = 0;
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            return velocity;
        }

        public virtual void HandleInput(InputHelper input)
        {
            this.Sprite.IsAnimating = false;
            this.gameScreen.Player.Velocity = this.Movement(input) * this.speed;

            // Turret Motion
            Vector2 facing = new Vector2(input.MouseState.X - this.Sprite.Position.X, input.MouseState.Y - this.Sprite.Position.Y);
            float angle = (float)Math.Atan2(facing.Y, facing.X) + MathHelper.PiOver2;
            int switchable = (int)(angle / (Math.PI / 8) + .5f);
            switch (switchable)
            {
                case 12:
                case -4:
                    this.Sprite.CurrentAnimation = AnimationKey.West;
                    break;
                case -3:
                    this.Sprite.CurrentAnimation = AnimationKey.West_NorthWest;
                    break;
                case -2:
                    this.Sprite.CurrentAnimation = AnimationKey.NorthWest;
                    break;
                case -1:
                    this.Sprite.CurrentAnimation = AnimationKey.North_NorthWest;
                    break;
                case 0:
                    this.Sprite.CurrentAnimation = AnimationKey.North;
                    break;
                case 1:
                    this.Sprite.CurrentAnimation = AnimationKey.North_NorthEast;
                    break;
                case 2:
                    this.Sprite.CurrentAnimation = AnimationKey.NorthEast;
                    break;
                case 3:
                    this.Sprite.CurrentAnimation = AnimationKey.East_NorthEast;
                    break;
                case 4:
                    this.Sprite.CurrentAnimation = AnimationKey.East;
                    break;
                case 5:
                    this.Sprite.CurrentAnimation = AnimationKey.East_SouthEast;
                    break;
                case 6:
                    this.Sprite.CurrentAnimation = AnimationKey.SouthEast;
                    break;
                case 7:
                    this.Sprite.CurrentAnimation = AnimationKey.South_SouthEast;
                    break;
                case 8:
                    this.Sprite.CurrentAnimation = AnimationKey.South;
                    break;
                case 9:
                    this.Sprite.CurrentAnimation = AnimationKey.South_SouthWest;
                    break;
                case 10:
                    this.Sprite.CurrentAnimation = AnimationKey.SouthWest;
                    break;
                case 11:
                    this.Sprite.CurrentAnimation = AnimationKey.West_SouthWest;
                    break;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Sprite.Update(gameTime);
        }
    }
}
