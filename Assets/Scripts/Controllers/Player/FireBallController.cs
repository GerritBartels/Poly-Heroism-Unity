using Controllers.Enemy;
using UnityEngine;

namespace Controllers.Player
{
    public class FireBallController : PlayerAttackControllerBase
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private GameObject explosionPrefab;
        private bool _destroied = false;

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
            if (_destroied) return;
            var obj = Instantiate(explosionPrefab, transform.position, transform.rotation);
            obj.GetComponent<PlayerAttackControllerBase>().Damage = Damage;
            _destroied = true;
            Destroy(gameObject);
        }

        protected override void OnEnemyContact(AbstractEnemyController enemy)
        {
            Destroy();
        }
    }
}