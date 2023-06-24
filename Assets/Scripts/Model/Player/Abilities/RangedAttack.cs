using UnityEngine;

namespace Model.Player.Abilities
{
    public class RangedAttack : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;
        private static readonly int Shot = Animator.StringToHash("shot");

        public RangedAttack(Transform transform, GameObject prefab, Animator animator) :
            base(cooldown: 0f, globalCooldown: 0.8f, resourceCost: 2f, blockMovementFor: 0.8f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        public override void PerformAbility()
        {
            Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f) + _transform.up,
                _transform.rotation);
        }

        protected override bool TriggerAnimation(PlayerModel player)
        {
            _animator.SetTrigger(Shot);
            return true;
        }

        protected override Resource GetResource(PlayerModel player)
        {
            return player.Mana;
        }
    }
}