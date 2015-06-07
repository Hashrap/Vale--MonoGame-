using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vale.GameObjects.Modifiers;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    /// Represents a GameActor that engages in combat.
    /// Notably, a CombatUnit has health, can receive and deal damage
    /// </summary>
    class CombatUnit : GameActor
    {
        /* events to consider
         * ModifierAdded
         * ModifierLost
         * Killed
         * LostHealth
         * GainedHealth
         * UsedSkill
         */

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

        private double health;

        private double maxHealth;


        //this probably needs to be more sophisticated
        // list of modifiers effecting this unit.
        //public float Speed { get; set; }
        private List<Modifier> modifiers;

        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
        }

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

        public double ReceiveDamage(GameActor source, double rawDamage)
        //maybe there should be a "Damage" struct which holds raw damage value, damage type, modifiers?
        {
            //apply modifers: damage increase/decrease %
            // if immune, do no damage
            health -= rawDamage;

            return rawDamage; //eventually return actual damage after mitigations/modifiers
        }

        public CombatUnit(GameplayScreen gameScreen, Faction alignment)
            : base(gameScreen, alignment)
        {
        }
    }
}
