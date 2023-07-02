using Controllers;
using UnityEngine;

namespace Model.Player.Abilities
{
    public class FireBall : AttackControllerAbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;

        public FireBall(Transform transform, GameObject prefab, Animator animator) :
            base(cooldown: 0.5f, globalCooldown: 0.5f, resourceCost: 10f, blockMovementFor: 0.5f, baseDamage: 50f)
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

        protected override Resource GetResource(PlayerModel player) => player.Mana;

        protected override float DamageModifier(PlayerModel playerModel) => playerModel.MagicDamageModificator();

        protected override GameObject InstantiateAttack()
        {
            return Instantiate(_prefab, _transform.position + (_transform.forward * 1f) + _transform.up,
                _transform.rotation);
        }
    }
}