using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyFireBallController : MonoBehaviour
    {
        [SerializeField] private float lifeSpan = 2f;
        private readonly float _speed = 10f;
        [SerializeField] private GameObject explosionPrefab;

        private void Update()
        {
            // Move Fireballs
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

            // Destroy
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Terrain") || other.CompareTag("Enemy") || other.CompareTag("Player"))
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}