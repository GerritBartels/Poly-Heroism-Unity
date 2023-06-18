namespace Model
{
    public class Enemy
    {
        public Resource Health { get; }

        public Enemy(float health)
        {
            Health = new Resource(0, health);
        }

        public bool IsAlive => !Health.Empty();

        public bool TakeDamage(float damage)
        {
            Health.Value -= damage;
            return !Health.Empty();
        }
    }
}