namespace Model.Player.Abilities
{
    public class NullAbility : IAbility<PlayerModel>
    {
        public bool Use(PlayerModel player)
        {
            return true;
        }

        public float CooldownTimeRemaining()
        {
            return 0f;
        }

        public NullAbility()
        {
            Cooldown = 0f;
            GlobalCooldown = 0f;
            ResourceCost = 0f;
        }

        public float Cooldown { get; }
        public float GlobalCooldown { get; }
        public float ResourceCost { get; }
    }
}