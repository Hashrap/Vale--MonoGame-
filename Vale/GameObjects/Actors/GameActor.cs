using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Modifiers;
using Vale.GameObjects.Skills;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    /// Represents any moving character.
    /// </summary>
    class GameActor : IUpdatable, IDrawable
    {
        public Game1 Game { get; private set; }
        public SpriteBatch SprtBatch { get; private set; }

        public enum ActorState { Standing, Moving, Dead } //this probably needs to be more sophisticated

        private ActorState state;

        private List<Modifier> modifiers; // list of modifiers effecting this unit. 
        
        public Vector2 Position { get; set; }

        public Vector2 PreviousPosition { get; set; }

        public double Speed { get; set; }

        private double maxHealth;

        private double health; // maybe attributes that can modifed by buffs, such as Health, should be in a special Attribute class?

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

        protected Texture2D texture;

        public GameActor(Game1 game, SpriteBatch spriteBatch)
        {
            this.Game = game;
            this.SprtBatch = spriteBatch;
        }

        public void Initialize(Texture2D tex, Vector2 pos)
        {
            texture = tex;
            Position = pos;
        }


        public virtual void Draw(GameTime gameTime)
        {
            SprtBatch.Draw(texture, Position, null, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public int DrawOrder { get; private set; }
        public bool Visible { get; private set; }
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public virtual void Update(GameTime gameTime)
        {

        }


        public bool Controllable
        {
            get { return true; } // eventually loop through and check for Stun modifiers? 
        }

        public double ReceiveDamage(GameActor source, double rawDamage) //maybe there should be a "Damage" struct which holds raw damage value, damage type, modifiers?
        {
            //apply modifers: damage increase/decrease %
            // if immune, do no damage
            health -= rawDamage;


            return rawDamage; //eventually return actual damage after mitigations/modifiers
        }
    }
}
