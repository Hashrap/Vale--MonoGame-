using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Skills
{
    public class SkillArgs
    {
        private CombatUnit unitOwner;
        private Player playerOwner;

        private CombatUnit targetUnit;
        private Vector2 targetPosition;
    }
    
    /// <summary>
    ///     Represents a skill that can be used by a CombatUnit
    /// </summary>
    internal abstract class Skill : IUpdate, IDraw
    {
        private Texture2D iconTexture;


        public virtual void LoadContent()
        {
            iconTexture = Game.Content.Load<Texture2D>("Art/quickshot10x20");
        }

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
            return true;
        }

        public bool ResetCooldown()
        {
            CooldownTimeRemaining = 0;
            return true;
        }


        public bool IsInteruptable { get; private set; }
        public bool IsCancellable { get; private set; }

        public enum SkillState
        {
            Available,
            Charging,
            OnCooldown
        }

        public SkillState Status { get; private set; }

        public CombatUnit Owner { get; private set; }



        public GameplayScreen Game;

        /// <summary>
        ///     Force children to use this constructor
        /// </summary>
        /// <param name="gameScreen">The currently active gameplay screen. Acts as sort of the root owner.</param>
        /// <param name="owner">The actor that owns this skill.</param>
        protected Skill(GameplayScreen gameScreen, CombatUnit owner)
        {
            Status = SkillState.Available;
            Owner = owner;
            Game = gameScreen;
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
        public bool TryExecute(SkillArgs args, params object[] list)
        {
            if (!IsReady || !Owner.Controllable)
                return false;


            Status = SkillState.Charging;
            DoAction(args, list);
            StartCooldown();
            return true;
        }



        /// <summary>
        ///     Updates the skill's recharge time based on elapsed gametime.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            CooldownTimeRemaining -= gameTime.ElapsedGameTime.Milliseconds;

            //elapsedChannelTime += gameTime.ElapsedGameTime.Milliseconds;
            /*
            if (elapsedChannelTime >= MaxChannelDuration)
            {
                ChannelEnded();
            }*/

            // if (cooldownTimeRemaining > Ready)
            {
                // cooldownRecharge -= gameTime.ElapsedGameTime.Milliseconds;
                //if (cooldownRecharge == Ready)
                //   Status = SkillState.Available;
            }
        }

        protected abstract bool DoAction(SkillArgs args, params object[] additionalParams);
        public Texture2D Texture { get; set; }
        public bool Visible { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw sprite effects?
        }
    }
}
