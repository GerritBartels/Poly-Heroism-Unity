using Controllers.Enemy;
using UnityEngine;

namespace Controllers
{
    public class ExplosionController : PlayerAttackControllerBase
    {
        private void Awake()
        {
            lifeSpan = 0.4f;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // Apply damage enemy
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<AbstractEnemyController>();
                enemy.TakeDamage(Damage);
                Destroy(gameObject.GetComponent<SphereCollider>());
            }
        }
    }
}