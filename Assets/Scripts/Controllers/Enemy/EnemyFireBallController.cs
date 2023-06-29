using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class EnemyFireBallController : MonoBehaviour
    {
        private Vector3 _direction;
        private Rigidbody _rigidbody;
        private Rigidbody _rigidbodyPlayer;
        [SerializeField] private float lifeSpan = 2f;
        private readonly float _speed = 10f;
        [SerializeField] private GameObject explosionPrefab;

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
            transform.Translate(_direction * (_speed * Time.deltaTime));

            // DESTROY
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