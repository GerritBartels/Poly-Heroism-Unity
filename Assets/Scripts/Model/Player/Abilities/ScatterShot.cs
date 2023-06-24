using UnityEngine;

namespace Model.Player.Abilities
{
    public class ScatterShot : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;
        private static readonly int Shot = Animator.StringToHash("scatterShot");

        public ScatterShot(Transform transform, GameObject prefab, Animator animator) :
            base(cooldown: 5f, globalCooldown: 2.7f, resourceCost: 20f, blockMovementFor: 2.7f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        public override void PerformAbility()
        {
            Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f) + _transform.up,
                _transform.rotation);
            Instantiate(
                _prefab,
                _transform.position + (_transform.forward * 0.5f) + (_transform.right * -0.3f) + _transform.up,
                _transform.rotation * Quaternion.Euler(0f, -10f, 0f)
            );
            Instantiate(
                _prefab,
                _transform.position + (_transform.forward * 0.5f) + (_transform.right * 0.3f) + _transform.up,
                _transform.rotation * Quaternion.Euler(0f, 10f, 0f)
            );
        }

        protected override bool TriggerAnimation(PlayerModel player)
        {
            _animator.SetTrigger(Shot);
            return true;
        }

        protected override Resource GetResource(PlayerModel player)
        {
            return player.Stamina;
        }
    }
}