using System.Collections;
using System.Collections.Generic;
using Model.Enemy;
using Model.Enemy.Abilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class BossEnemyController : EnemyController<EnemyBasic>
    {
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private float distanceToPlayer = 4f;

        private void Update()
        {
            if (DistanceToPlayer() > distanceToPlayer)
            {
                MoveTowardsPlayer();
            }

            Enemy.Attack();
        }

        protected override EnemyBasic CreateEnemy()
        {
            return new EnemyBasic(300, new InfernoAttack(fireballPrefab, transform, SelfToPlayerVector), 1f);
        }
    }
}