using System;
using UnityEngine;

namespace Model.Enemy.Abilities
{
    public class RangedAttack : AbilityBase
    {
        private readonly GameObject _bulletPrefab;
        private readonly Transform _transform;

        public RangedAttack(GameObject bulletPrefab, Transform transform) :
            base(2f, 2f)
        {
            _bulletPrefab = bulletPrefab;
            _transform = transform;
        }

        protected override bool PerformAbility(Enemy enemy)
        {
            Instantiate(_bulletPrefab, _transform.position + _transform.forward + _transform.up, _transform.rotation);
            return true;
        }
    }
}