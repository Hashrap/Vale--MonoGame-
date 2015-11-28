using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Modifiers;
using Vale.Parsing;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    /// Represents a GameActor that engages in combat.
    /// Notably, a CombatUnit has health, can receive and deal damage
    /// </summary>
    public class CombatUnit : MoveableGameObject
    {
        /* events to consider
         * ModifierAdded
         * ModifierLost
         * Killed
         * LostHealth
         * GainedHealth
         * UsedSkill
         */
        private UnitInfo defaultInfo;

        public enum Faction
        {
            Player,
            Hostile,
            Neutral
        }

        public Faction Alignment { set; get; }

        public enum ActorState
        {
            Standing,
            Moving,
            Dead
        }

        protected ActorState state;

        public bool Controllable
        {
            get { return true; } // eventually loop through and check for Stun modifiers?
        }

        private float speed;

        /// <summary>
        ///     The magnitude of this game object's velocity
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = Math.Max(0.0f, value); }
        }


        private float health;

        private float maxHealth;


        //this probably needs to be more sophisticated
        // list of modifiers effecting this unit.
        //public float Speed { get; set; }
        private List<Buff> modifiers;

        public void AddModifier(Buff buff)
        {
            modifiers.Add(buff);
        }

        public float Health
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

        public double ReceiveDamage(CombatUnit source, float rawDamage)
        //maybe there should be a "Damage" struct which holds raw damage value, damage type, modifiers?
        {
            //apply modifers: damage increase/decrease %
            // if immune, do no damage
            health -= rawDamage;

            return rawDamage; //eventually return actual damage after mitigations/modifiers
        }

        public CombatUnit(GameplayScreen gameScreen, Faction alignment, Vector2 spawn, Vector2 size)
            : base(gameScreen, spawn, size)
        {
            this.Alignment = alignment;
        }

        public void LoadUnitInfo(UnitInfo info)
        {
            defaultInfo = info;
            SetupUnit();
        }

        private void SetupUnit()
        {
            this.Health = defaultInfo.Health;
            this.Speed = defaultInfo.Speed;

            //set-up abilities here
        }

        public override void LoadContent()
        {
            this.Texture = Game.Content.Load<Texture2D>("Art/arrow20x20.png");
            base.LoadContent();
        }
    }
}
