using Model.Enemy;
using Model.Enemy.Abilities;
using UnityEngine;

namespace Controllers.Enemy
{
    public class BossEnemyController : EnemyController<EnemyBoss>
    {
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private GameObject homingMissilePrefab;
        [SerializeField] private float distanceToPlayer = 4f;

        private void Update()
        {
            RotateTowardsPlayer();
            Enemy.Attack();
        }

        private void FixedUpdate()
        {
            if (DistanceToPlayer() > distanceToPlayer)
            {
                MoveTowardsPlayer();
            }
        }

        protected override EnemyBoss CreateEnemy(int lvl)
        {
            return new EnemyBoss(500, new InfernoAttack(fireballPrefab, transform),
                new HomingMissilesAttack(homingMissilePrefab, transform), 1f, lvl);
        }
    }
}