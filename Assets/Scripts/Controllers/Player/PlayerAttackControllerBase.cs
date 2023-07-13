using Controllers.Enemy;
using UnityEngine;

namespace Controllers.Player
{
    public abstract class PlayerAttackControllerBase : MonoBehaviour
    {
        public float Damage { get; set; }

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

        protected virtual void OnTriggerEnter(Collider other)
        {
            // Apply damage enemy
            if (other.CompareTag("Enemy"))
            {
                OnEnemyContact(other.GetComponent<AbstractEnemyController>());
            }
            else if (other.CompareTag("Terrain") || other.CompareTag("EnemyBullet"))
            {
                Destroy();
            }
        }


        protected virtual void Destroy()
        {
            Destroy(gameObject);
        }

        protected virtual void OnEnemyContact(AbstractEnemyController enemy)
        {
            enemy.TakeDamage(Damage);
            Destroy();
        }
    }
}