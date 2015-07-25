using Microsoft.Xna.Framework;
using System;

namespace Vale.GameObjects.Modifiers
{
    public enum EffectState
    {
        Dormant,
        Active,
        Expired
    }

    /// <summary>
    ///     A buff is a status effect on a combat unit. Buffs may hold different modifiers and affect their host in a positive, negative, or neutral fashion.
    /// </summary>
    public class Buff : IUpdate // AKA BUFF/DEBUFF
    {
        private readonly int duration;
        private int elapsedTime; //MODIFIER KEEPS TRACK OF ITSELF, LIKE PROJECTILES?

        // THIS SHOULD PROBABLY BE A LIST SO THAT MODIFIERS CAN DO MORE THAN ONE THING?
        private EffectState state;

        public GameActor.Faction Alignment { get; private set; }

        //Event when expired?
        public bool Expired
        {
            get { return state == EffectState.Expired; }
        }

        public Buff(int duration, GameActor.Faction alignment)
        {
            this.duration = duration;
            this.Alignment = alignment;
            elapsedTime = 0;
            state = EffectState.Dormant;
        }

        public void Activate()
        {
            state = EffectState.Active;
        }

        public void Update(GameTime gameTime)
        {
            if (state == EffectState.Active)
            {
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedTime > duration)
                    state = EffectState.Expired;
            }
        }
    }
}