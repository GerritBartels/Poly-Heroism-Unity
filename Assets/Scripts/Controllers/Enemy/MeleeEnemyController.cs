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

        public PlayerController PlayerController
        {
            get
            {
                if (_playerController is null)
                {
                    _playerController = player.GetComponent<PlayerController>();
                }

                return _playerController;
            }
        }

        private new void Start()
        {
            base.Start();
        }

        protected override EnemyBasic CreateEnemy(int lvl)
        {
            return new EnemyBasic(150, new MeleeAttack(DistanceToPlayer, PlayerController, 15f), 4f, lvl);
        }

        private void Update()
        {
            RotateTowardsPlayer();
            Enemy.Attack();
        }

        private void FixedUpdate()
        {
            if (DistanceToPlayer() > 1.3)
            {
                MoveTowardsPlayer();
            }
        }
    }
}