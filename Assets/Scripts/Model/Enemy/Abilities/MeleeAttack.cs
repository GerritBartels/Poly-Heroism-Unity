using System;

namespace Model.Enemy.Abilities
{
    public class MeleeAttack : AbilityBase
    {
        private readonly Func<float> _distanceToPlayer;
        private readonly float _range;

        public MeleeAttack(Func<float> distanceToPlayer, float range = 3f) : base(1f, 1f)
        {
            _distanceToPlayer = distanceToPlayer;
            _range = range;
        }

        protected override bool PerformAbility(Enemy enemy)
        {
            return _distanceToPlayer() < _range;
        }
    }
}