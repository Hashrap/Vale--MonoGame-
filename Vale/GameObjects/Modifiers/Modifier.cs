using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vale.GameObjects.Modifiers
{
    enum EffectType { Stun, Slow, Root, Disarm } // EXPAND UPON LATER

    internal enum EffectState
    {
        Dormant,
        Active,
        Expired
    }

    /// <summary>
    /// A modifier is buff/debuff that applies an effect to its owner
    /// </summary>
    class Modifier : IUpdateable // AKA BUFF/DEBUFF
    {
        private readonly int duration;
        private int elapsedTime; //MODIFIER KEEPS TRACK OF ITSELF, LIKE PROJECTILES?
        private EffectType effectType; // THIS SHOULD PROBABLY BE A LIST SO THAT MODIFIERS CAN DO MORE THAN ONE THING?
        private EffectState state;
        //Event when expired?


        public Modifier(EffectType effectType, int duration)
        {
            this.effectType = effectType;
            this.duration = duration;
            elapsedTime = 0;

            state = EffectState.Dormant;
        }

        public void Activate()
        {
            state = EffectState.Active;
        }


        public bool Expired { get { return state == EffectState.Expired; } }


        public EffectType Effect { get { return effectType; } }
        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > duration)
                state = EffectState.Expired;
        }

        public bool Enabled { get; private set; }
        public int UpdateOrder { get; private set; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
    }
}
