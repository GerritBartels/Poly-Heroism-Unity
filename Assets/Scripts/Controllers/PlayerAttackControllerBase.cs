using Controllers.Enemy;
using UnityEngine;

namespace Controllers
{
    public abstract class PlayerAttackControllerBase : MonoBehaviour
    {
        [SerializeField] protected float damage = 10f;

        [SerializeField] protected float lifeSpan = 1f;

        public void Update()
        {
            // destroy if lifespan expired
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Apply damage enemy
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<AbstractEnemyController>();
                enemy.TakeDamage(damage);
            }
            else if (other.CompareTag("Terrain"))
            {
                Destroy(gameObject);
            }
        }
    }
}