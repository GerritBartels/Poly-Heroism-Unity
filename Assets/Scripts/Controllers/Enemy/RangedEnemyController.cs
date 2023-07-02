using System.Collections;
using System.Collections.Generic;
using Model.Enemy;
using Model.Enemy.Abilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class RangedEnemyController : EnemyController<EnemyBasic>
    {
        [SerializeField] private GameObject bulletPrefab;
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
            else
            {
                MoveAwayFromPlayer();
            }
        }

        protected override EnemyBasic CreateEnemy()
        {
            return new EnemyBasic(100, new RangedAttack(bulletPrefab, transform), 2f);
        }
    }
}