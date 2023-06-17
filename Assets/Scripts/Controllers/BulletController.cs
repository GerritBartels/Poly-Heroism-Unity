using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private Rigidbody RB;

        [SerializeField] private float bulletSpeed = 10f;
        private float _lifeSpan = 4f;
        private Rigidbody _rigidbody;
        private Vector3 _direction;

        [SerializeField] private LayerMask layersToHit;


        private void Start()
        {
            var mousePos = Input.mousePosition;
            // mousePos.z = Camera.main.nearClipPlane + 1;
            // mousePos = Camera.main.ScreenToWorldPoint((mousePos));
            // var rigidbodyPlayer = GameObject.Find("Player").GetComponent<Rigidbody>();
            var targetPos = Vector3.zero;
            var ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out var hitData, 50, layersToHit))
            {
                targetPos = hitData.point;
            }

            _rigidbody = GetComponent<Rigidbody>();
            var position = _rigidbody.position;
            _direction = (targetPos - position).normalized;
        }

        public void Update()
        {
            // SHOOT BULLET
            transform.Translate(Vector3.forward * (bulletSpeed * Time.deltaTime));

            // DESTROY
            _lifeSpan -= Time.deltaTime;
            if (_lifeSpan <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}