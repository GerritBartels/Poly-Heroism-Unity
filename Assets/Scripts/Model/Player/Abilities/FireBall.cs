using UnityEngine;

namespace Model.Player.Abilities
{
    public class FireBall : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;

        public FireBall(Transform transform, GameObject prefab, Animator animator) : base(0f, 0.5f, 10f, 0.5f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        public void PerformAnimation()
        {
            _animator.SetTrigger("fireBall"); // TODO: add animation
        }

        protected override bool PerformAbility(PlayerModel player)
        {
            Instantiate(_prefab, _transform.position + (_transform.forward * 1f) + _transform.up, _transform.rotation);
            return true;
        }

        protected override Resource GetResource(PlayerModel player)
        {
            return player.Mana;
        }
    }
}