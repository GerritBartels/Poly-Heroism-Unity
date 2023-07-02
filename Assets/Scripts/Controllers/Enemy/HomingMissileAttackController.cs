using UnityEngine;

namespace Controllers.Enemy
{
    public class HomingMissileAttackController : MonoBehaviour
    {
        private Rigidbody _playerRigidbody;
        [SerializeField] private float lifeSpan = 4f;
        private readonly float _speed = 9f;

        private void Start()
        {
            _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        }

        private void Update()
        {
            // Rotate whilst keeping orientation perpendicular to the planet
            Vector3 up = transform.position.normalized;
            Vector3 targetDir = _playerRigidbody.position.normalized;
            Vector3 forward = targetDir - up * Vector3.Dot(targetDir, up);
            if (forward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(forward.normalized, up.normalized);
            }

            // Move HomingMissile
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
            if (other.CompareTag("Player"))
            {
                other.GetComponentInParent<PlayerController>().Damage(10);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Terrain") || other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}