using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public abstract class EnemyController<T> : AbstractEnemyController where T : Model.Enemy.Enemy
    {
        [SerializeField] protected float baseDamage = 25f;

        [SerializeField] protected GameObject player;

        private Rigidbody _rigidbody;
        private Rigidbody _rigidbodyPlayer;
        public T Enemy { get; private set; }

        protected void Start()
        {
            Enemy = CreateEnemy();
            _rigidbodyPlayer = player.GetComponent<Rigidbody>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void OnCollisionEnter(Collision other)
        {
            // Prevent enemies from falling through the planet
            if (other.gameObject.CompareTag("Bedrock"))
            {
                transform.position += (transform.up * 0.25f);

            }
        }

        protected abstract T CreateEnemy();

        public override void TakeDamage(float damage)
        {
            if (!Enemy.TakeDamage(damage))
            {
                OnDeath();
            }
        }

        protected virtual void OnDeath()
        {
            Destroy(gameObject);
        }

        protected void RotateTowardsPlayer()
        {
            // Rotate whilst keeping orientation perpendicular to the planet
            Vector3 up = transform.position.normalized;
            Vector3 targetDir = _rigidbodyPlayer.position.normalized;
            Vector3 forward = targetDir - up * Vector3.Dot(targetDir, up);
            transform.rotation = Quaternion.LookRotation(forward.normalized, up.normalized);
        }

        protected void MoveTowardsPlayer()
        {
            _rigidbody.MovePosition(_rigidbody.position + transform.forward * (Enemy.Speed * Time.deltaTime));
        }

        protected void MoveAwayFromPlayer()
        {
            _rigidbody.MovePosition(_rigidbody.position - transform.forward * (Enemy.Speed * Time.deltaTime));
        }

        protected Vector3 SelfToPlayerVector()
        {
            var position = _rigidbody.position;
            var playerPosition = _rigidbodyPlayer.position;
            return playerPosition - position;
        }

        protected float DistanceToPlayer()
        {
            return SelfToPlayerVector().magnitude;
        }
    }
}