using Controllers.Enemy;
using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyExplosionController : MonoBehaviour
    {
        [SerializeField] protected float damage = 50f;

        [SerializeField] protected float lifeSpan = 0.4f;

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
            // Apply damage to player
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponentInParent<PlayerController>();
                player.Damage(damage);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Terrain") || other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}