using System;
using static System.Random;

namespace Model.Enemy
{
    public class EnemyBoss : Enemy
    {
        private readonly IAbility<Enemy> _inferno;
        private readonly IAbility<Enemy> _homingMissiles;
        private readonly Random _rnd = new();
        private readonly Cooldown _globalCooldown = new();

        public EnemyBoss(float health, IAbility<Enemy> inferno, IAbility<Enemy> homingMissiles, float speed, int lvl) :
            base(health, speed, lvl)
        {
            _inferno = inferno;
            _homingMissiles = homingMissiles;
        }

        public bool Attack()
        {
            if (_globalCooldown.IsCooldownActive()) return false;
            if (Convert.ToBoolean(_rnd.Next(0, 2)))
            {
                if (!_homingMissiles.Use(this)) return false;
                _globalCooldown.Apply(_homingMissiles.GlobalCooldown);
                return true;
            }

            if (!_inferno.Use(this)) return false;
            _globalCooldown.Apply(_inferno.GlobalCooldown);
            return true;
        }
    }
}