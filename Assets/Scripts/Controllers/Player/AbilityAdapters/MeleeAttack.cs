using Model;
using Model.Player;
using UnityEngine;

namespace Controllers.Player.AbilityAdapters
{
    public class MeleeAttack : AttackControllerAbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;

        public MeleeAttack(Transform transform, GameObject prefab, Animator animator) :
            base(cooldown: .025f, globalCooldown: 0.25f, resourceCost: 2f, blockMovementFor: 0.2f, baseDamage: 50f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        protected override bool TriggerAnimation(PlayerModel player)
        {
            // TODO: add actual animation + trigger and call animator here
            base.PerformAbility(player);
            return true;
        }

        protected override Resource GetResource(PlayerModel player) => player.Stamina;

        protected override float DamageModifier(PlayerModel playerModel) => playerModel.PhysicalDamageModificator();

        protected override GameObject InstantiateAttack()
        {
            return Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f), _transform.rotation);
        }
    }
}