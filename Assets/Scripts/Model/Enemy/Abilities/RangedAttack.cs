using System;
using UnityEngine;

namespace Model.Enemy.Abilities
{
    public class RangedAttack : AttackControllerAbilityBase
    {
        private readonly GameObject _bulletPrefab;
        private readonly Transform _transform;

        public RangedAttack(GameObject bulletPrefab, Transform transform) :
            base(2f, 2f, 5f)
        {
            _bulletPrefab = bulletPrefab;
            _transform = transform;
        }

        protected override GameObject InstantiateAttack(Enemy enemy)
        {
            return Instantiate(_bulletPrefab, _transform.position + _transform.forward + _transform.up,
                _transform.rotation);
        }
    }
}