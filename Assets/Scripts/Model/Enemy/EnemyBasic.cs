namespace Model.Enemy
{
    public class EnemyBasic : Enemy
    {
        private readonly IAbility<Enemy> _ability;

        public EnemyBasic(float health, IAbility<Enemy> ability, float speed = 10f) : base(health, speed)
        {
            _ability = ability;
        }

        public bool Attack()
        {
            return _ability.Use(this);
        }
    }
}