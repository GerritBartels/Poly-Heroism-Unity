using Controllers.Player;
using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyFireBallController : EnemyAttackControllerBase
    {
        [SerializeField] private float lifeSpan = 2f;
        private readonly float _speed = 10f;
        [SerializeField] private GameObject explosionPrefab;

        private bool _destroyed = false;

        private void Update()
        {
            // Move Fireballs
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

            // Destroy
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0)
            {
                Destroy(gameObject);
            }
        }

        protected override void Destroy()
        {
            if (_destroyed) return;
            var obj = Instantiate(explosionPrefab, transform.position, transform.rotation);
            obj.GetComponent<EnemyAttackControllerBase>().Damage = Damage;
            _destroyed = true;
            Destroy(gameObject);
        }

        protected override void OnPlayerContact(PlayerController player)
        {
            Destroy();
        }
    }
}