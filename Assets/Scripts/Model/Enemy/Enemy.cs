namespace Model.Enemy
{
    public class Enemy
    {
        public Resource Health { get; }

        private readonly CooldownFixed _blockMovement = new(0.2f);

        private readonly float _baseSpeed;
        public float Speed => _blockMovement.IsCooldownActive() ? 0f : _baseSpeed;

        private readonly int _lvl = 0;

        public Enemy(float health, float speed, int lvl)
        {
            _lvl = lvl - 1;
            _baseSpeed = speed;
            Health = new Resource(0f, health * LvlToScalingFactor());
        }

        public bool IsAlive => !Health.Empty();

        public bool TakeDamage(float damage)
        {
            Health.Value -= damage;
            return !Health.Empty();
        }

        public void BlockMovement()
        {
            _blockMovement.Apply();
        }

        private float LvlToScalingFactor() => 1f + _lvl / 100f;

        public float DamageModifier() => LvlToScalingFactor();
    }
}