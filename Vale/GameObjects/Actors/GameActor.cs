using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Vale.GameObjects.Modifiers;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    ///     Represents any moving character.
    /// </summary>
    internal class GameActor : MoveableGameObject
    {
        public enum ActorState
        {
            Standing,
            Moving,
            Dead
        }

        protected Texture2D texture;

        private double health;

        private double maxHealth;

        private List<Modifier> modifiers;

        private float spriteWidth = 20, spriteHeight = 20;

        private ActorState state;

        public bool Controllable
        {
            get { return true; } // eventually loop through and check for Stun modifiers?
        }

        public int DrawOrder { get; private set; }

        public double Health
        {
            get { return health; }
            set
            {
                if (value <= 0) // we dead
                {
                    health = 0;
                    state = ActorState.Dead;
                }
                else // not dead yet
                {
                    health = Math.Min(value, maxHealth);
                }
            }
        }

        public Vale.ScreenSystem.GameScreen GameScreen { get; set; }

        //this probably needs to be more sophisticated
        // list of modifiers effecting this unit.
        //public float Speed { get; set; }

        public SpriteBatch SprtBatch { get; private set; }

        public bool Visible { get; private set; }

        protected Vector2 DrawingOrigin
        {
            get { return new Vector2(spriteWidth / 2, spriteHeight / 2); }
        }

        protected Vector2 DrawingPosition
        {
            get { return Position - DrawingOrigin; }
        }

        // maybe attributes that can modifed by buffs, such as Health, should be in a special Attribute class?
        public GameActor(Vale.ScreenSystem.GameScreen gameScreen)
        {
            GameScreen = gameScreen;
        }

        public virtual void LoadContent() { }

        public virtual void Draw(GameTime gameTime)
        {
            GameScreen.SpriteBatch.Draw(texture, DrawingPosition, null, Color.White, Rotation, DrawingOrigin, 1f, SpriteEffects.None,
                0f);
        }

        public double ReceiveDamage(GameActor source, double rawDamage)
        //maybe there should be a "Damage" struct which holds raw damage value, damage type, modifiers?
        {
            //apply modifers: damage increase/decrease %
            // if immune, do no damage
            health -= rawDamage;

            return rawDamage; //eventually return actual damage after mitigations/modifiers
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}