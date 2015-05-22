﻿using Microsoft.Xna.Framework;
using Vale.GameObjects.Actors;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    /// Represents a skill that can be used by a Hero or Enemy
    /// </summary>
    internal abstract class Skill : IUpdatable
    {
        private const int Ready = 0;

        private readonly int cooldown;
        private int cooldownRecharge;

        public GameActor Owner { get; private set; }

        /// <summary>
        /// Force children to use this constructor
        /// </summary>
        /// <param name="owner">The actor that owns this skill.</param>
        protected Skill(GameActor owner)
        {
            this.Owner = owner;
            cooldown = 5;
            //can set default cooldown here?
        }

        /// <summary>
        /// Is this skill still recharging?
        /// </summary>
        /// <returns></returns>
        public bool OnCooldown
        {
            get { return cooldownRecharge > Ready; }
        }

        /// <summary>
        /// Execute the ability.
        /// </summary>
        /// <param name="list">The list of parameters necessary for the skill.</param>
        /// <returns>Returns success.</returns>
        public bool Execute(params object[] list)
        {
            if (OnCooldown) return false;

            DoAction(list);
            BeginCooldown();
            return true;
        }

        protected abstract bool DoAction(params object[] list);

        /// <summary>
        /// Puts the skill on cooldown.
        /// </summary>
        private void BeginCooldown()
        {
            cooldownRecharge = cooldown;
        }

        /// <summary>
        /// Resets the cooldown, making the skill instantly available for use.
        /// </summary>
        public void ResetCooldown()
        {
            cooldownRecharge = Ready;
        }

        /// <summary>
        /// Updates the skill's recharge time based on elapsed gametime.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (cooldownRecharge > Ready)
                cooldownRecharge -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
