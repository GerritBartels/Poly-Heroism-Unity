using UnityEngine;

namespace Model.Abilities
{
    public class MeleeAttack : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;

        public MeleeAttack(Transform transform, GameObject prefab) : base(0, 1, 2)
        {
            _transform = transform;
            _prefab = prefab;
        }

        protected override bool PerformAbility(Player player)
        {
            Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f), _transform.rotation);
            return true;
        }

        protected override Resource GetResource(Player player)
        {
            return player.Stamina;
        }
    }
}