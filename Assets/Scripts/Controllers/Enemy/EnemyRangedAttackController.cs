using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyRangedAttackController : MonoBehaviour
    {
        [SerializeField] private float lifeSpan = 2f;
        [SerializeField] private float bulletSpeed = 50f;

        private void Update()
        {
            // Move bullets
            transform.Translate(Vector3.forward * (bulletSpeed * Time.deltaTime));

            // Destroy
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponentInParent<PlayerController>().Damage(5);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Terrain") || other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}