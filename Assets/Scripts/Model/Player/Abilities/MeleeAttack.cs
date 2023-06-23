using UnityEngine;

namespace Model.Player.Abilities
{
    public class MeleeAttack : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;

        public MeleeAttack(Transform transform, GameObject prefab, Animator animator) : base(0f, 0.2f, 2f, 0.2f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        public void PerformAnimation()
        {
            _animator.SetTrigger("meleeAttack"); // TODO: add animation
        }

        protected override bool PerformAbility(PlayerModel player)
        {
            Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f), _transform.rotation);
            return true;
        }

        protected override Resource GetResource(PlayerModel player)
        {
            return player.Stamina;
        }
    }
}