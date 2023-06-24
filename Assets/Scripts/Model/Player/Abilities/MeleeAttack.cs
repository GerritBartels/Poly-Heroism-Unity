using UnityEngine;

namespace Model.Player.Abilities
{
    public class MeleeAttack : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;

        public MeleeAttack(Transform transform, GameObject prefab, Animator animator) :
            base(cooldown: 0f, globalCooldown: 0.2f, resourceCost: 2f, blockMovementFor: 0.2f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        public override void PerformAbility()
        {
            Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f), _transform.rotation);
        }

        protected override bool TriggerAnimation(PlayerModel player)
        {
            // TODO: add actual animation + trigger and call animator here
            Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f), _transform.rotation);
            return true;
        }

        protected override Resource GetResource(PlayerModel player)
        {
            return player.Stamina;
        }
    }
}