using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class MeleeAttackController : MonoBehaviour
    {
        private float _lifeSpan = 0.1f;

        public void Update()
        {
            // DESTROY
            _lifeSpan -= Time.deltaTime;
            if (_lifeSpan <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // DESTROY ENEMY
            if (other.CompareTag("Enemy"))
            {
                var enemyGameObject = other.GetComponent<EnemyController>().gameObject;
                Destroy(enemyGameObject);
            }
        }
    }
}