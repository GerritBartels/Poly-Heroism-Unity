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
            if (DistanceToPlayer() > distanceToPlayer)
            {
                MoveTowardsPlayer();
            }

            Enemy.Attack();
        }

        protected override EnemyBoss CreateEnemy()
        {
            return new EnemyBoss(300, new InfernoAttack(fireballPrefab, transform, SelfToPlayerVector),
                new HomingMissilesAttack(homingMissilePrefab, transform), 1f);
        }
    }
}