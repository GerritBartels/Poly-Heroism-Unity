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
                OnPlayerContact(other.GetComponentInParent<PlayerController>());
            }
            else if (other.CompareTag("Terrain") || other.CompareTag("Bullet"))
            {
                Destroy();
            }
        }

        protected virtual void Destroy()
        {
            Destroy(gameObject);
        }

        protected virtual void OnPlayerContact(PlayerController player)
        {
            player.Damage(Damage);
            Destroy();
        }
    }
}