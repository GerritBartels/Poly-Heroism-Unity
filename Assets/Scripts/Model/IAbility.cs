namespace Model
{
    public interface IAbility<in T>
    {
        public bool Use(T target);
        public float CooldownTimeRemaining();
        public float Cooldown { get; }
        public float GlobalCooldown { get; }
    }
}