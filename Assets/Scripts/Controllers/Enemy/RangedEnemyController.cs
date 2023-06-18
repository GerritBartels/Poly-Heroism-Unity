using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class RangedEnemyController : EnemyController
    {
        [SerializeField] private GameObject bulletPrefab;
        private float _distanceToPlayer = 4f;
        private const float FireCoolDownTime = 1f;
        private float _nextFireTime = 0.5f;

        private void Update()
        {
            if (DistanceToPlayer() > _distanceToPlayer)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveAwayFromPlayer();
            }

            //fire bullet
            if (_nextFireTime < Time.time)
            {
                Instantiate(bulletPrefab, transform.position + SelfToPlayerVector().normalized, transform.rotation);
                _nextFireTime = Time.time + FireCoolDownTime;
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