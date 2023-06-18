using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class SuicideEnemyController : EnemyController
    {
        private void Update()
        {
            MoveTowardsPlayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            // DAMAGE PLAYER
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().Damage(damage);
                Destroy(this.gameObject);
            }

            // DESTROY ENEMY + BULLET
            if (other.CompareTag("Bullet"))
            {
                Destroy(this.gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}