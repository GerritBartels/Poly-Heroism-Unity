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

        protected void MoveTowardsPlayer()
        {
            var position = _rigidbody.position;
            var moveDirection = SelfToPlayerVector().normalized;
            _rigidbody.MovePosition(position +
                                    transform.TransformDirection(moveDirection) * (Enemy.Speed * Time.deltaTime));
        }

        protected void MoveAwayFromPlayer()
        {
            var position = _rigidbody.position;
            var moveDirection = -SelfToPlayerVector().normalized;
            _rigidbody.MovePosition(position +
                                    transform.TransformDirection(moveDirection) * (Enemy.Speed * Time.deltaTime));
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