using Unity.IO.LowLevel.Unsafe;

namespace Model.Abilities
{
    public interface IAbility
    {
        public bool Use(Player player);
        public float ActiveCooldown();
        public float CoolDown { get; }
        public float GlobalCooldown { get; }
        public float ResourceCost { get; }
    }
}