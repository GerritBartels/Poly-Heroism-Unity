using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class EnemyRangedAttackController : MonoBehaviour
    {
        private Vector3 _direction;
        private Rigidbody _rigidbody;
        private Rigidbody _rigidbodyPlayer;
        [SerializeField] private float lifeSpan = 2f;
        [SerializeField] private float bulletSpeed = 50f;

        private void Start()
        {
            _rigidbodyPlayer = GameObject.Find("Player").GetComponent<Rigidbody>();
            _rigidbody = GetComponent<Rigidbody>();
            var position = _rigidbody.position;
            var playerPosition = _rigidbodyPlayer.position;
            _direction = (playerPosition - position).normalized;
        }

        private void Update()
        {
            // SHOOT BULLET
            transform.Translate(_direction * (bulletSpeed * Time.deltaTime));

            // DESTROY
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
            else if (other.CompareTag("Terrain"))
            {
                Destroy(gameObject);
            }
        }
    }
}