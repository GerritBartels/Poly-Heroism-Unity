using Model.Enemy;
using UnityEngine;

namespace Controllers.Enemy.AbilityAdapters
{
    public abstract class AttackControllerAbilityBase : AbilityBase
    {
        protected AttackControllerAbilityBase(float cooldown, float globalCooldown, float damage) :
            base(cooldown, globalCooldown, damage)
        {
        }

        protected override bool PerformAbility(Model.Enemy.Enemy enemy)
        {
            var obj = InstantiateAttack(enemy);
            var controller = obj.GetComponent<EnemyAttackControllerBase>();
            controller.Damage = Damage(enemy);
            return true;
        }

        protected abstract GameObject InstantiateAttack(Model.Enemy.Enemy enemy);
    }
}