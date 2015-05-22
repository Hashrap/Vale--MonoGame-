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
        public enum ActorState { Standing, Moving, Dead } //this probably needs to be more sophisticated

        private ActorState state;

        private List<Modifier> modifiers; // list of modifiers effecting this unit. 

        private List<Skill> skillSet; // might not be the best way to store skills?

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

        public void Initialize(Texture2D tex, Vector2 pos)
        {
            texture = tex;
            Position = pos;
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public void ExecuteSkill(int skillIndex, params object[] list)
        {
            skillSet[skillIndex].Execute(list);
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
