using UnityEngine;

namespace Controllers.Enemy
{
    public abstract class AbstractEnemyController : MonoBehaviour
    {
        public abstract void TakeDamage(float damage);
        public abstract Model.Enemy.Enemy GetEnemy();
        [SerializeField] public GameObject player;
    }
}