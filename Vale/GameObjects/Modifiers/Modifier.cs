using Microsoft.Xna.Framework;
using System;

namespace Vale.GameObjects.Modifiers
{
    internal enum EffectState
    {
        Dormant,
        Active,
        Expired
    }

    internal enum EffectType
    {
        Stun,
        Slow,
        Root,
        Disarm
    } // EXPAND UPON LATER

    /// <summary>
    ///     A modifier is buff/debuff that applies an effect to its owner
    /// </summary>
    internal class Modifier : IUpdateable // AKA BUFF/DEBUFF
    {
        private readonly int duration;
        private EffectType effectType;
        private int elapsedTime; //MODIFIER KEEPS TRACK OF ITSELF, LIKE PROJECTILES?

        // THIS SHOULD PROBABLY BE A LIST SO THAT MODIFIERS CAN DO MORE THAN ONE THING?
        private EffectState state;

        //Event when expired?

        public EffectType Effect
        {
            get { return effectType; }
        }

        public bool Enabled { get; private set; }

        public bool Expired
        {
            get { return state == EffectState.Expired; }
        }

        public int UpdateOrder { get; private set; }

        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

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

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > duration)
                state = EffectState.Expired;
        }
    }
}