using Controllers.Enemy;
using Controllers.Player;
using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyExplosionController : EnemyAttackControllerBase
    {
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

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponentInParent<PlayerController>().Damage(Damage);
                Destroy(gameObject.GetComponent<SphereCollider>());
            }
        }
    }
}