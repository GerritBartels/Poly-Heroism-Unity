using System.Collections;
using System.Collections.Generic;
using Model.Enemy;
using Model.Enemy.Abilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class SuicideEnemyController : EnemyController<EnemyBasic>
    {
        private PlayerController _playerController;

        private new void Start()
        {
            base.Start();
            baseDamage = 25;
            _playerController = player.GetComponent<PlayerController>();
        }

        protected override EnemyBasic CreateEnemy()
        {
            return new EnemyBasic(50, new MeleeAttack(DistanceToPlayer, 1f), 4f);
        }

        private void Update()
        {
            MoveTowardsPlayer();
            if (Enemy.Attack())
            {
                _playerController.Damage(baseDamage);
                Destroy(gameObject);
            }
        }
    }
}