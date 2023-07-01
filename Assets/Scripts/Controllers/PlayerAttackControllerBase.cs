using Controllers.Enemy;
using UnityEngine;

namespace Controllers
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
                var enemy = other.GetComponent<AbstractEnemyController>();
                Debug.Log("damage enemy in controller:" + Damage);
                enemy.TakeDamage(Damage);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Terrain"))
            {
                Destroy(gameObject);
            }
        }
    }
}