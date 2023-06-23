using UnityEngine;

namespace Model.Player.Abilities
{
    public class ScatterShot : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;

        public ScatterShot(Transform transform, GameObject prefab, Animator animator) : base(5f, 0.5f, 20f, 0.5f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        public void PerformAnimation()
        {
            _animator.SetTrigger("scatterShot");
        }

        protected override bool PerformAbility(PlayerModel player)
        {
            Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f) + _transform.up, _transform.rotation);
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
            return true;
        }

        protected override Resource GetResource(PlayerModel player)
        {
            return player.Stamina;
        }
    }
}