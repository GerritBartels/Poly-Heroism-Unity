using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class HomingMissileAttackController : MonoBehaviour
    {
        private Vector3 _direction;
        private Rigidbody _rigidbody;
        private Rigidbody _playerRigidbody;
        [SerializeField] private float lifeSpan = 4f;
        private readonly float _speed = 15f;

        private void Start()
        {
            _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private Vector3 ToPlayerVec()
        {
            var position = _rigidbody.position;
            var playerPosition = _playerRigidbody.position;
            _direction = (playerPosition - position).normalized;
            return _direction;
        }

        private void Update()
        {
            // move HomingMissile
            transform.Translate(ToPlayerVec() * (_speed * Time.deltaTime));

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
                other.GetComponentInParent<PlayerController>().Damage(20);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Terrain") || other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}