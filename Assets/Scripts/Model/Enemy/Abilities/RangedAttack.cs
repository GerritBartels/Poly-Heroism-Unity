using System;
using UnityEngine;

namespace Model.Enemy.Abilities
{
    public class RangedAttack : AbilityBase
    {
        private readonly GameObject _bulletPrefab;
        private readonly Transform _transform;
        private readonly Func<Vector3> _selfToPlayerVector;

        public RangedAttack(GameObject bulletPrefab, Transform transform, Func<Vector3> selfToPlayerVector) :
            base(1f, 1f)
        {
            _bulletPrefab = bulletPrefab;
            _transform = transform;
            _selfToPlayerVector = selfToPlayerVector;
        }

        protected override bool PerformAbility(Enemy enemy)
        {
            Instantiate(_bulletPrefab, _transform.position + _selfToPlayerVector().normalized, _transform.rotation);
            return true;
        }
    }
}