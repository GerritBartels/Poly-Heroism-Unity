using System.Collections;
using System.Collections.Generic;
using Model.Enemy;
using Model.Enemy.Abilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

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

        protected override EnemyBoss CreateEnemy()
        {
            return new EnemyBoss(1000, new InfernoAttack(fireballPrefab, transform),
                new HomingMissilesAttack(homingMissilePrefab, transform), 1f);
        }
    }
}