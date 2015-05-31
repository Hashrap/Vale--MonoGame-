using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    ///     Represents a skill that can be used by a Hero or Enemy
    /// </summary>
    internal abstract class Skill : IUpdate, IDraw
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

        public int DrawOrder { get; private set; }

        public bool Enabled { get; private set; }

        /// <summary>
        ///     Is this skill still recharging?
        /// </summary>
        /// <returns></returns>
        public bool OnCooldown
        {
            get { return cooldownRecharge > Ready; }
        }

        public GameActor Owner { get; private set; }

        public int UpdateOrder { get; private set; }

        public bool Visible { get; private set; }

        protected GameScreen GameScreen
        {
            get { return Owner.GameScreen; }
        }

        protected SpriteBatch SpriteBatch
        {
            get { return Owner.SprtBatch; }
        }


        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        ///     Force children to use this constructor
        /// </summary>
        /// <param name="owner">The actor that owns this skill.</param>
        protected Skill(GameScreen gameScreen, GameActor owner)
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

        public virtual void Draw(GameTime gameTime)
        {
        }
        /// <summary>
        ///     Execute the ability.
        /// </summary>
        /// <param name="list">The list of parameters necessary for the skill.</param>
        /// <returns>Returns success.</returns>
        public bool Execute(params object[] list)
        {
            if (OnCooldown) return false;
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
        public virtual void Update(GameTime gameTime)
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