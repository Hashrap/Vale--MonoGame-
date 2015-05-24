using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;

namespace Vale.GameObjects.Skills
{
    /// <summary>
    /// Represents a skill that can be used by a Hero or Enemy
    /// </summary>
    internal abstract class Skill : IUpdatable, IDrawable
    {
        enum SkillTimeline { Available, InUse, OnCooldown}

        // maybe there should be a SKillPool class that holds all of the different ability types and gives them to GameActors by cloning them?

        private SkillTimeline Status;

        private const int Ready = 0;

        private readonly int cooldown;
        private int cooldownRecharge;

        public GameActor Owner { get; private set; }
        protected Game1 Game { get { return Owner.Game;} }
        protected SpriteBatch SpriteBatch { get { return Owner.SprtBatch; } }

        /// <summary>
        /// Force children to use this constructor
        /// </summary>
        /// <param name="owner">The actor that owns this skill.</param>
        protected Skill(Game1 game, SpriteBatch spriteBatch, GameActor owner)
        {
            Status = SkillTimeline.Available;
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
            Status = SkillTimeline.InUse;
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
            Status = SkillTimeline.OnCooldown;
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
        public virtual void Update(GameTime gameTime)
        {
            if (cooldownRecharge > Ready)
            {
                cooldownRecharge -= gameTime.ElapsedGameTime.Milliseconds;
                if(cooldownRecharge == Ready)
                    Status = SkillTimeline.Available;
            }
        } 
        /// <summary>
        /// Begins the action. Disables other commands while using this action (channeling for shots).
        /// </summary>
        public void BeginAction()
        {
            
        }

        public virtual void Draw(GameTime gameTime)
        {
            
        }

        public int DrawOrder { get; private set; }
        public bool Visible { get; private set; }
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
    }
}
