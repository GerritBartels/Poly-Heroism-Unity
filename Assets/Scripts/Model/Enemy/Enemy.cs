using UnityEngine;

namespace Model.Enemy
{
    public class Enemy
    {
        public Resource Health { get; }

        private readonly CooldownFixed _blockMovement = new(0.2f);

        private readonly float _baseSpeed;
        public float Speed => _blockMovement.IsCooldownActive() ? 0f : _baseSpeed;

        public Enemy(float health, float speed = 10f)
        {
            _baseSpeed = speed;
            Health = new Resource(0f, health);
        }

        public bool IsAlive => !Health.Empty();

        public bool TakeDamage(float damage)
        {
            Health.Value -= damage;
            return !Health.Empty();
        }

        public void BlockMovement()
        {
            _blockMovement.Apply();
        }
    }
}