namespace Model
{
    public interface IAbility<in T>
    {
        public bool Use(T target);
        public float CooldownTimeRemaining();
        public float CooldownTime { get; }
        public float GlobalCooldown { get; }
        public Cooldown Cooldown { get; }
    }
}