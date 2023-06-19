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
            if (DistanceToPlayer() > distanceToPlayer)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveAwayFromPlayer();
            }

            Enemy.Attack();
        }

        protected override EnemyBasic CreateEnemy()
        {
            return new EnemyBasic(100, new RangedAttack(bulletPrefab, transform, SelfToPlayerVector), 2f);
        }
    }
}