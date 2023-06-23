namespace Model.Player.Abilities
{
    public abstract class AbilityBase : UnityEngine.Object, IAbility<PlayerModel>
    {
        private CooldownFixed _cooldown;
        private readonly float _blockMovementFor;

        protected AbilityBase(float cooldown, float globalCooldown, float resourceCost, float blockMovementFor)
        {
            Cooldown = cooldown;
            GlobalCooldown = globalCooldown;
            ResourceCost = resourceCost;
            _blockMovementFor = blockMovementFor;
            _cooldown = new CooldownFixed(cooldown);
        }

        public float Cooldown { get; }
        public float GlobalCooldown { get; }
        public float ResourceCost { get; }

        protected abstract bool PerformAbility(PlayerModel playerModel);
        protected abstract Resource GetResource(PlayerModel playerModel);

        public float CooldownTimeRemaining()
        {
            return _cooldown.RemainingTime();
        }

        public bool Use(PlayerModel playerModel)
        {
            var resource = GetResource(playerModel);
            if (!_cooldown.IsCooldownActive() && resource.Value >= ResourceCost)
            {
                if (PerformAbility(playerModel))
                {
                    resource.Value -= ResourceCost;
                    _cooldown.Apply();
                    playerModel.BlockMovement(_blockMovementFor);
                    return true;
                }
            }

            return false;
        }
    }
}