using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        [SerializeField] protected float damage = 25f;

        [SerializeField] protected GameObject player;

        private Rigidbody _rigidbody;
        private Rigidbody _rigidbodyPlayer;

        protected void Start()
        {
            _rigidbodyPlayer = player.GetComponent<Rigidbody>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        protected void MoveTowardsPlayer()
        {
            var position = _rigidbody.position;
            var moveDirection = SelfToPlayerVector().normalized;
            _rigidbody.MovePosition(position + transform.TransformDirection(moveDirection) * (speed * Time.deltaTime));
        }

        protected void MoveAwayFromPlayer()
        {
            var position = _rigidbody.position;
            var moveDirection = SelfToPlayerVector().normalized;
            moveDirection = new Vector3(-moveDirection.x, -moveDirection.y, -moveDirection.z);
            _rigidbody.MovePosition(position + transform.TransformDirection(moveDirection) * (speed * Time.deltaTime));
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