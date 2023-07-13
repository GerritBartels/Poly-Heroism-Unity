using Controllers.Player;
using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyAttackControllerBase : MonoBehaviour
    {
        public float Damage { get; set; }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponentInParent<PlayerController>().Damage(Damage);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Terrain") || other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}