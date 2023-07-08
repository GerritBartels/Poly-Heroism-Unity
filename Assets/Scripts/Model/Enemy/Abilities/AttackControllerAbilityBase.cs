using Controllers.Enemy;
using UnityEngine;

namespace Model.Enemy.Abilities
{
    public abstract class AttackControllerAbilityBase : AbilityBase
    {
        protected AttackControllerAbilityBase(float cooldown, float globalCooldown, float damage) :
            base(cooldown, globalCooldown, damage)
        {
        }

        protected override bool PerformAbility(Enemy enemy)
        {
            var obj = InstantiateAttack(enemy);
            var controller = obj.GetComponent<EnemyAttackControllerBase>();
            controller.Damage = Damage(enemy);
            return true;
        }

        protected abstract GameObject InstantiateAttack(Enemy enemy);
    }
}