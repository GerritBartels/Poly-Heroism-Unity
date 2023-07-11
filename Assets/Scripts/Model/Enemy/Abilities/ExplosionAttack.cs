using System;
using Controllers;
using UnityEngine;

namespace Model.Enemy.Abilities
{
    public class ExplosionAttack : AttackControllerAbilityBase
    {
        private readonly Func<float> _distanceToPlayer;
        private readonly float _range;
        private readonly PlayerController _player;
        private readonly GameObject _explosionPrefab;
        private readonly Transform _transform;

        public ExplosionAttack(Func<float> distanceToPlayer, PlayerController player, GameObject explosionPrefab, Transform transform,
            float damage = 10f, float range = 3f)
            : base(1f, 1f, damage)
        {
            _transform = transform;
            _explosionPrefab = explosionPrefab;
            _player = player;
            _distanceToPlayer = distanceToPlayer;
            _range = range;
        }

        protected override bool PerformAbility(Enemy enemy)
        {
            return _distanceToPlayer() < _range && base.PerformAbility(enemy);
        }

        protected override GameObject InstantiateAttack(Enemy enemy)
        {
            //_player.Damage(Damage(enemy));
            return Instantiate(_explosionPrefab, _transform.position, _transform.rotation);
        }
    }
}