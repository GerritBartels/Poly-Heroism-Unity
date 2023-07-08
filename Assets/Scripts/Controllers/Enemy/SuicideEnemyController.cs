using Model.Enemy;
using Model.Enemy.Abilities;

namespace Controllers.Enemy
{
    public class SuicideEnemyController : EnemyController<EnemyBasic>
    {
        private PlayerController _playerController;

        public PlayerController PlayerController
        {
            get
            {
                if (_playerController is null)
                {
                    _playerController = player.GetComponent<PlayerController>();
                }

                return _playerController;
            }
        }

        protected override EnemyBasic CreateEnemy(int lvl)
        {
            return new EnemyBasic(50, new MeleeAttack(DistanceToPlayer, PlayerController, 25f, 1f), 4f, lvl);
        }

        private void Update()
        {
            RotateTowardsPlayer();
            if (Enemy.Attack())
            {
                Enemy.TakeDamage(Enemy.Health.MaxValue);
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            MoveTowardsPlayer();
        }
    }
}