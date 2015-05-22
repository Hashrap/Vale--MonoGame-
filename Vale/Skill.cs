namespace Vale
{
    internal abstract class Skill
    {
        private const int Ready = 0;

        private readonly double cooldown;
        private double cooldownRecharge;

        /// <summary>
        /// Force children to use this constructor
        /// </summary>
        /// <param name="list"></param>
        protected Skill(params object[] list)
        {
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

        protected virtual bool DoAction(params object[] list)
        {
            // do things using list of params
            return true;
        }

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
