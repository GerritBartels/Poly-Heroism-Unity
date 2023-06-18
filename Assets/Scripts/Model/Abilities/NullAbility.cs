namespace Model.Abilities
{
    public class NullAbility : IAbility
    {
        public bool Use(Player player)
        {
            return true;
        }

        public float ActiveCooldown()
        {
            return 0f;
        }

        public NullAbility()
        {
            CoolDown = 0f;
            GlobalCooldown = 0f;
            ResourceCost = 0f;
        }

        public float CoolDown { get; }
        public float GlobalCooldown { get; }
        public float ResourceCost { get; }
    }
}