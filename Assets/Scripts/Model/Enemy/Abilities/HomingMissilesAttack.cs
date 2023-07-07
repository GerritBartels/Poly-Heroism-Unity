using Controllers.Enemy;
using UnityEngine;

namespace Model.Enemy.Abilities
{
    public class HomingMissilesAttack : AbilityBase
    {
        private readonly GameObject _missilePrefab;
        private readonly Transform _transform;

        public HomingMissilesAttack(GameObject bulletPrefab, Transform transform) :
            base(3f, 3f, 10f)
        {
            _missilePrefab = bulletPrefab;
            _transform = transform;
        }

        protected override bool PerformAbility(Enemy enemy)
        {
            var obj1 = Instantiate(_missilePrefab,
                _transform.position + _transform.forward * 4f + _transform.up * 3f,
                _transform.rotation);
            var obj2 = Instantiate(_missilePrefab,
                _transform.position + _transform.forward * 4f + (_transform.right * -2f) + _transform.up * 3f,
                _transform.rotation * Quaternion.Euler(0f, -10f, 0f));
            var obj3 = Instantiate(_missilePrefab,
                _transform.position + _transform.forward * 4f + (_transform.right * 2f) + _transform.up * 3f,
                _transform.rotation * Quaternion.Euler(0f, 10f, 0f));

            var controller1 = obj1.GetComponent<EnemyAttackControllerBase>();
            controller1.Damage = Damage(enemy);

            var controller2 = obj2.GetComponent<EnemyAttackControllerBase>();
            controller2.Damage = Damage(enemy);

            var controller3 = obj3.GetComponent<EnemyAttackControllerBase>();
            controller3.Damage = Damage(enemy);

            return true;
        }
    }
}