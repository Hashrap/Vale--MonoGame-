using System;
using Microsoft.Xna.Framework;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    ///     Represents a skill that can be used by a Hero or Enemy
    /// </summary>
    internal abstract class Skill : GameObject
    {
            private int cooldownTimeRemaining;
            public int CooldownTime { get; private set; }
            
        public bool IsReady
            {
                get { return cooldownTimeRemaining <= 0; }
            }

            /// <summary>
            /// Gets the remaining cooldown time of the ability, in milliseconds.
            /// </summary>
            public int CooldownTimeRemaining
            {
                get { return cooldownTimeRemaining; }
                private set { cooldownTimeRemaining = Math.Max(0, value); }
            }

            public bool StartCooldown()
            {
                CooldownTimeRemaining = CooldownTime;
            }

            public bool ResetCooldown()
            {
                CooldownTimeRemaining = 0;
            }
        
        
            public bool IsChanneling { get; private set; }
            public bool IsInteruptable { get; private set; }
            public bool IsCancellable { get; private set; }
            public int MaxChannelDuration { get; set; }
            private int elapsedChannelTime;

            public void ChannelStarted()
            {
                IsChanneling = true;
                elapsedChannelTime = 0;
            }
        

            public void ChannelEnded()
            {
                IsChanneling = false;
            }
        

        public enum SkillState
        {
            Available,
            Charging,
            OnCooldown
        }

        public SkillState Status { get; private set; }

        public CombatUnit Owner { get; private set; }



        /// <summary>
        ///     Force children to use this constructor
        /// </summary>
        /// <param name="gameScreen">The currently active gameplay screen. Acts as sort of the root owner.</param>
        /// <param name="owner">The actor that owns this skill.</param>
        protected Skill(GameplayScreen gameScreen, CombatUnit owner)
            : base(gameScreen)
        {
            Status = SkillState.Available;
            Owner = owner;
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
            if (!IsReady || !Owner.Controllable)
                return false;


            Status = SkillState.Charging;
            DoAction(list);
            //StartCooldown();
            return true;
        }



        /// <summary>
        ///     Updates the skill's recharge time based on elapsed gametime.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            CooldownTimeRemaining -= gameTime.ElapsedGameTime.Milliseconds;

            elapsedChannelTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedChannelTime >= MaxChannelDuration)
            {
                ChannelEnded();
            }

            if (cooldownRecharge > Ready)
            {
                cooldownRecharge -= gameTime.ElapsedGameTime.Milliseconds;
                if (cooldownRecharge == Ready)
                    Status = SkillState.Available;
            }
        }

        protected abstract bool DoAction(params object[] list);

    }
}
