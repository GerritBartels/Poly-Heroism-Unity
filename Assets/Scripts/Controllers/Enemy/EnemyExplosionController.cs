using Controllers.Enemy;
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
    }
}