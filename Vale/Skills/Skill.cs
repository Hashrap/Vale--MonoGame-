namespace Vale.Skills
{
    internal abstract class Skill
    {
        private const int Ready = 0;

        private readonly double cooldown;
        private double cooldownRecharge;

        public GameActor Owner { get; private set; }

        /// <summary>
        /// Force children to use this constructor
        /// </summary>
        /// <param name="owner">The actor that owns this skill.</param>
        protected Skill(GameActor owner)
        {
            this.Owner = owner;
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

        private void BeginCooldown()
        {
            cooldownRecharge = cooldown;
        }

        public void ResetCooldown()
        {
            cooldownRecharge = Ready;
        }
    }
}
