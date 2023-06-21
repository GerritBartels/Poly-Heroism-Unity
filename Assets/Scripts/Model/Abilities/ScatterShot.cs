using UnityEngine;

namespace Model.Abilities
{
    public class ScatterShot : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;

        public ScatterShot(Transform transform, GameObject prefab) : base(5f, 1f, 20f)
        {
            _transform = transform;
            _prefab = prefab;
        }

        protected override bool PerformAbility(Player player)
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

        protected override Resource GetResource(Player player)
        {
            return player.Stamina;
        }
    }
}