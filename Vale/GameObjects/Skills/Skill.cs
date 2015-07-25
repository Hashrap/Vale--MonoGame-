﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    ///     Represents a skill that can be used by a Hero or Enemy
    /// </summary>
    internal abstract class Skill : GameObject
    {
        private enum SkillTimeline
        {
            Available,
            InUse,
            OnCooldown
        }

        private const int Ready = 0;
        private readonly int cooldown;
        private int cooldownRecharge;
        private SkillTimeline Status;

        /// <summary>
        ///     Is this skill still recharging?
        /// </summary>
        /// <returns></returns>
        public bool OnCooldown
        {
            get { return cooldownRecharge > Ready; }
        }

        public CombatUnit Owner { get; private set; }

        protected GameplayScreen GameScreen
        {
            get { return Owner.Screen; }
        }

        /// <summary>
        ///     Force children to use this constructor
        /// </summary>
        /// <param name="gameScreen">The currently active gameplay screen. Acts as sort of the root owner.</param>
        /// <param name="owner">The actor that owns this skill.</param>
        protected Skill(GameplayScreen gameScreen, CombatUnit owner)
            : base(gameScreen)
        {
            Status = SkillTimeline.Available;
            Owner = owner;
            cooldown = 5;
            //can set default cooldown here?
        }

        /// <summary>
        ///     Begins the action. Disables other commands while using this action (channeling for shots).
        /// </summary>
        public void BeginAction()
        {
        }

        /// <summary>
        ///     Execute the ability.
        /// </summary>
        /// <param name="list">The list of parameters necessary for the skill.</param>
        /// <returns>Returns success.</returns>
        public bool Execute(params object[] list)
        {
            if (OnCooldown || !Owner.Controllable)
                return false;


            Status = SkillTimeline.InUse;
            DoAction(list);
            BeginCooldown();

            return true;
        }

        /// <summary>
        ///     Resets the cooldown, making the skill instantly available for use.
        /// </summary>
        public void ResetCooldown()
        {
            cooldownRecharge = Ready;
        }

        /// <summary>
        ///     Updates the skill's recharge time based on elapsed gametime.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (cooldownRecharge > Ready)
            {
                cooldownRecharge -= gameTime.ElapsedGameTime.Milliseconds;
                if (cooldownRecharge == Ready)
                    Status = SkillTimeline.Available;
            }
        }

        protected abstract bool DoAction(params object[] list);

        /// <summary>
        ///     Puts the skill on cooldown.
        /// </summary>
        private void BeginCooldown()
        {
            Status = SkillTimeline.OnCooldown;
            cooldownRecharge = cooldown;
        }
    }
}
