using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class MeleeEnemyController : EnemyController
    {
        private const float AttackCoolDownTime = 1f;
        private float _nextAttackTime = 0.1f;
        private PlayerController _playerController;

        private new void Start()
        {
            base.Start();
            _playerController = player.GetComponent<PlayerController>();
        }

        private void Update()
        {
            MoveTowardsPlayer();

            if (DistanceToPlayer() < 1f)
            {
                if (_nextAttackTime < Time.time)
                {
                    _playerController.Damage(damage);
                    _nextAttackTime = Time.time + AttackCoolDownTime;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // DESTROY ENEMY + BULLET
            if (other.CompareTag("Bullet"))
            {
                Destroy(this.gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}