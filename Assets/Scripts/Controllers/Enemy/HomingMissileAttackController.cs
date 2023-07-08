using UnityEngine;

namespace Controllers.Enemy
{
    public class HomingMissileAttackController : EnemyAttackControllerBase
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
            var up = transform.position.normalized;
            var targetDir = _playerRigidbody.position.normalized;
            var forward = targetDir - up * Vector3.Dot(targetDir, up);
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
        
    }
}