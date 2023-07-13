namespace Model.Player
{
    public abstract class AbilityBase : UnityEngine.Object, IAbility<PlayerModel>
    {
        private readonly CooldownFixed _cooldown;
        private readonly float _blockMovementFor;
        private readonly float _baseDamage;

        protected AbilityBase(float cooldown, float globalCooldown, float resourceCost, float blockMovementFor,
            float baseDamage)
        {
            _baseDamage = baseDamage;
            CooldownTime = cooldown;
            GlobalCooldown = globalCooldown;
            ResourceCost = resourceCost;
            _blockMovementFor = blockMovementFor;
            _cooldown = new CooldownFixed(cooldown);
        }

        public float CooldownTime { get; }
        public float GlobalCooldown { get; }
        public Cooldown Cooldown => _cooldown;
        public float ResourceCost { get; }

        public abstract void PerformAbility(PlayerModel playerModel);

        protected abstract bool TriggerAnimation(PlayerModel playerModel);
        protected abstract Resource GetResource(PlayerModel playerModel);

        public float CooldownTimeRemaining() => _cooldown.RemainingTime();

        protected abstract float DamageModifier(PlayerModel playerModel);

        protected float Damage(PlayerModel playerModel) => _baseDamage * DamageModifier(playerModel);

        public bool Use(PlayerModel playerModel)
        {
            var resource = GetResource(playerModel);
            if (_cooldown.IsCooldownActive() || !(resource.Value >= ResourceCost)) return false;
            if (!TriggerAnimation(playerModel)) return false;
            resource.Value -= ResourceCost;
            _cooldown.Apply();
            playerModel.BlockMovement(_blockMovementFor);
            return true;
        }
    }
}