using System.Collections;
using System.Collections.Generic;
using Model.Enemy;
using Model.Enemy.Abilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class MeleeEnemyController : EnemyController<EnemyBasic>
    {
        private PlayerController _playerController;

        private new void Start()
        {
            base.Start();
            baseDamage = 15;
            _playerController = player.GetComponent<PlayerController>();
        }

        protected override EnemyBasic CreateEnemy()
        {
            return new EnemyBasic(150, new MeleeAttack(DistanceToPlayer, 2f), 4f);
        }

        private void Update()
        {
            RotateTowardsPlayer();
            if (Enemy.Attack())
            {
                _playerController.Damage(baseDamage);
            }
        }

        private void FixedUpdate()
        {
            MoveTowardsPlayer();
        }
    }
}