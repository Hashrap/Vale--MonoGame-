using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Vale.ScreenSystem;

namespace Vale.Components
{
    public class Player
    {
        public List<Character> Party;
        private GameplayScreen gameScreen;
        private Rectangle boundingBox;
        private Vector2 position;

        public Npc waitingOnEvent;

        public Player(GameplayScreen gameScreen)
        {
            this.gameScreen = gameScreen;
            this.CurrentCharacter = new Character(gameScreen);
            this.NextPosition = Vector2.Zero;
        }

        public Character CurrentCharacter { get; set; }

        // The Player has a position in Game-land
        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                this.boundingBox.X = (int)(value.X + .5f);
                this.boundingBox.Y = (int)(value.Y + .5f);
            }
        }

        public Vector2 Velocity { get; set; }

        public Vector2 NextPosition { get; set; }

        public Rectangle BoundingBox
        {
            get { return this.boundingBox; }
        }

        public void NextCharacter()
        {

        }

        public void PreviousCharacter()
        {

        }

        public void LoadContent(ContentManager content)
        {
            this.CurrentCharacter.LoadContent(content, "warden");
            this.boundingBox = new Rectangle(0, 0, this.CurrentCharacter.Sprite.Width, this.CurrentCharacter.Sprite.Height);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.IsNewKeyPress(Keys.Q))
            {
                this.PreviousCharacter();
            }
            else if (inputHelper.IsNewKeyPress(Keys.E))
            {
                this.NextCharacter();
            }

            if (this.waitingOnEvent != null)
            {
                if (inputHelper.IsNewKeyRelease(Keys.E))
                {
                    this.waitingOnEvent.TriggerEvent();
                }
            }

            this.CurrentCharacter.HandleInput(inputHelper);
        }

        public void Update(GameTime gameTime)
        {
            if (this.NextPosition == Vector2.Zero)
            {
                Vector2 position = this.Position;
                position += this.Velocity;
                this.Position =
                    new Vector2(
                        MathHelper.Clamp(position.X, 0, gameScreen.GameMap.WorldWidth - this.BoundingBox.Width),
                        MathHelper.Clamp(position.Y, 0, gameScreen.GameMap.WorldHeight - this.BoundingBox.Height)
                    );
            }
            else
            {
                this.Position = this.NextPosition;
                this.NextPosition = Vector2.Zero;
            }
            this.CurrentCharacter.Update(gameTime);
        }

        internal void InteractionEventDetect(Npc npc)
        {
            this.waitingOnEvent = npc;
        }

        internal void ClearEvent()
        {
            this.waitingOnEvent = null;
        }
    }
}