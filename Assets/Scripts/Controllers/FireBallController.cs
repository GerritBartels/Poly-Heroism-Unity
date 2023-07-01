using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class FireBallController : PlayerAttackControllerBase
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private GameObject explosionPrefab;

        private void Awake()
        {
            lifeSpan = 4f;
        }

        public new void Update()
        {
            base.Update();
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Terrain"))
            {
                var obj = Instantiate(explosionPrefab, transform.position, transform.rotation);
                obj.GetComponent<PlayerAttackControllerBase>().Damage = Damage;
                Destroy(gameObject);
            }
        }
    }
}