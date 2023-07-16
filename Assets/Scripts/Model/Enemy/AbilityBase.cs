namespace Model.Enemy
{
    public abstract class AbilityBase : UnityEngine.Object, IAbility<Enemy>
    {
        private readonly CooldownFixed _cooldown;
        public Cooldown Cooldown => _cooldown;

        private readonly float _damage;

        protected AbilityBase(float cooldown, float globalCooldown, float damage)
        {
            _damage = damage;
            CooldownTime = cooldown;
            GlobalCooldown = globalCooldown;
            _cooldown = new CooldownFixed(cooldown);
        }

        public float Damage(Enemy enemy) => _damage * enemy.DamageModifier();

        public float CooldownTime { get; }
        public float GlobalCooldown { get; }

        protected abstract bool PerformAbility(Enemy enemy);

        public float CooldownTimeRemaining()
        {
            return _cooldown.RemainingTime();
        }

        public bool Use(Enemy enemy)
        {
            if (!_cooldown.IsCooldownActive())
            {
                if (PerformAbility(enemy))
                {
                    _cooldown.Apply();
                    enemy.BlockMovement();
                    return true;
                }
            }

            return false;
        }
    }
}