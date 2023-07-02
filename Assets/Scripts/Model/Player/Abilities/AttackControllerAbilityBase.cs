using Controllers;
using UnityEngine;

namespace Model.Player.Abilities
{
    public abstract class AttackControllerAbilityBase : AbilityBase
    {
        protected AttackControllerAbilityBase(float cooldown, float globalCooldown, float resourceCost,
            float blockMovementFor, float baseDamage) : base(cooldown, globalCooldown, resourceCost,
            blockMovementFor, baseDamage)
        {
        }

        public override void PerformAbility(PlayerModel playerModel)
        {
            var obj = InstantiateAttack();
            var controller = obj.GetComponent<PlayerAttackControllerBase>();
            controller.Damage = Damage(playerModel);
        }

        protected abstract GameObject InstantiateAttack();
    }
}