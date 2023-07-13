using System;
using Controllers.Enemy;
using UnityEngine;

namespace Controllers.Player
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

        protected override void Destroy()
        {
            var obj = Instantiate(explosionPrefab, transform.position, transform.rotation);
            obj.GetComponent<PlayerAttackControllerBase>().Damage = Damage;
            Destroy(gameObject);
        }

        protected override void OnEnemyContact(AbstractEnemyController enemy)
        {
            Destroy();
        }
    }
}