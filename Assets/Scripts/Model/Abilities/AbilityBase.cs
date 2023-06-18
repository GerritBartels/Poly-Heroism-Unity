using UnityEngine;

namespace Model.Abilities
{
    public abstract class AbilityBase : UnityEngine.Object, IAbility
    {
        private float _lastActivation;

        protected AbilityBase(float coolDown, float globalCooldown, float resourceCost)
        {
            CoolDown = coolDown;
            GlobalCooldown = globalCooldown;
            ResourceCost = resourceCost;
            _lastActivation = 0f;
        }

        public float CoolDown { get; }
        public float GlobalCooldown { get; }
        public float ResourceCost { get; }

        protected abstract bool PerformAbility(Player player);
        protected abstract Resource GetResource(Player player);

        public float ActiveCooldown()
        {
            var remainingCooldown = CoolDown - (Time.time - _lastActivation);
            return remainingCooldown >= 0f ? remainingCooldown : 0f;
        }

        public bool CooldownExpired()
        {
            return ActiveCooldown() <= 0f;
        }

        public bool Use(Player player)
        {
            var resource = GetResource(player);
            if (CooldownExpired() && resource.Value >= ResourceCost)
            {
                if (PerformAbility(player))
                {
                    resource.Value -= ResourceCost;
                    _lastActivation = Time.time;
                    return true;
                }
            }

            return false;
        }
    }
}