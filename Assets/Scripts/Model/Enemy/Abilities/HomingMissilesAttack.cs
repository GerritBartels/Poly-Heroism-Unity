using System;
using UnityEngine;

namespace Model.Enemy.Abilities
{
    public class HomingMissilesAttack : AbilityBase
    {
        private readonly GameObject _missilePrefab;
        private readonly Transform _transform;
        private readonly Func<Vector3> _selfToPlayerVector;

        public HomingMissilesAttack(GameObject bulletPrefab, Transform transform) :
            base(3f, 3f)
        {
            _missilePrefab = bulletPrefab;
            _transform = transform;
        }

        protected override bool PerformAbility(Enemy enemy)
        {
            Instantiate(_missilePrefab, _transform.position + _transform.forward * 4f + _transform.up * 6f,
                _transform.rotation);
            Instantiate(_missilePrefab, _transform.position + _transform.forward * 4f + _transform.up * 6f,
                _transform.rotation * Quaternion.Euler(0f, -10f, 0f));
            Instantiate(_missilePrefab, _transform.position + _transform.forward * 4f + _transform.up * 6f,
                _transform.rotation * Quaternion.Euler(0f, 10f, 0f));
            return true;
        }
    }
}