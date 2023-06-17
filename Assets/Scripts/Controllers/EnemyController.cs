using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        [SerializeField] private GameObject player;

        [SerializeField] private GameObject bulletPrefab;

        private const float FireCoolDownTime = 1f;
        private float _nextFireTime = 0.5f;
        private Rigidbody _rigidbody;
        private Rigidbody _rigidbodyPlayer;
        private Rigidbody _rigidbody1;

        private void Start()
        {
            _rigidbodyPlayer = player.GetComponent<Rigidbody>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            // move towards player
            var position = _rigidbody.position;
            var playerPosition = _rigidbodyPlayer.position;
            var moveDirection = (playerPosition - position).normalized;
            _rigidbody.MovePosition(position + transform.TransformDirection(moveDirection) * (speed * Time.deltaTime));

            //fire bullet
            if (_nextFireTime < Time.time)
            {
                Instantiate(bulletPrefab, transform.position + moveDirection, transform.rotation);
                _nextFireTime = Time.time + FireCoolDownTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // DESTROY ENEMY + BULLET
            if (other.CompareTag("Bullet"))
            {
                Destroy(this.gameObject);
                Destroy(other.gameObject);
            }

            // DAMAGE PLAYER
            if (other.CompareTag("Player"))
            {
                Destroy(this.gameObject);
                other.GetComponent<PlayerController>().Damage(25);
            }
        }
    }
}