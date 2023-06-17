using UnityEditor;
using UnityEngine;

namespace Model.Abilities
{
    public abstract class AbilityBase : IAbility
    {
        protected abstract bool PerformAbility(Player player);
        protected abstract Resource GetResource(Player player);

        public bool Use(Player player)
        {
            var resource = GetResource(player);
            if (resource.Value >= ResourceCost)
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

        private float _lastActivation;

        public AbilityBase(float coolDown, float globalCooldown, float resourceCost)
        {
            CoolDown = coolDown;
            GlobalCooldown = globalCooldown;
            ResourceCost = resourceCost;
            _lastActivation = Time.time;
        }

        public float CoolDown { get; }
        public float GlobalCooldown { get; }
        public float ResourceCost { get; }

        public float ActiveCooldown()
        {
            var remainingCooldown = CoolDown - Time.time - _lastActivation;
            return remainingCooldown >= 0f ? remainingCooldown : 0f;
        }
    }
}