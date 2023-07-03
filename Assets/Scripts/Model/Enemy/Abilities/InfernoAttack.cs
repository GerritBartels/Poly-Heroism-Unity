using System;
using UnityEngine;

namespace Model.Enemy.Abilities
{
    public class InfernoAttack : AbilityBase
    {
        private readonly GameObject _fireBallPrefab;
        private readonly Transform _transform;

        public InfernoAttack(GameObject fireBallPrefab, Transform transform) :
            base(3f, 3f)
        {
            _fireBallPrefab = fireBallPrefab;
            _transform = transform;
        }

        protected override bool PerformAbility(Enemy enemy)
        {
            Instantiate(_fireBallPrefab, _transform.position + _transform.forward * 3f + _transform.up,
                _transform.rotation);
            Instantiate(
                _fireBallPrefab,
                _transform.position + _transform.forward * 3f + (_transform.right * -2f) + _transform.up,
                _transform.rotation * Quaternion.Euler(0f, -10f, 0f)
            );
            Instantiate(
                _fireBallPrefab,
                _transform.position + _transform.forward * 3f + (_transform.right * 2f) + _transform.up,
                _transform.rotation * Quaternion.Euler(0f, 10f, 0f)
            );
            return true;
        }
    }
}