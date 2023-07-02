using UnityEngine;

namespace Model.Player.Abilities
{
    public class RangedAttack : AttackControllerAbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;
        private static readonly int Shot = Animator.StringToHash("shot");

        public RangedAttack(Transform transform, GameObject prefab, Animator animator) :
            base(cooldown: 0.8f, globalCooldown: 0.8f, resourceCost: 2f, blockMovementFor: 0.8f, baseDamage: 15f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        protected override bool TriggerAnimation(PlayerModel player)
        {
            _animator.SetTrigger(Shot);
            return true;
        }

        protected override Resource GetResource(PlayerModel player) => player.Mana;

        protected override float DamageModifier(PlayerModel playerModel) => playerModel.MagicDamageModificator();

        protected override GameObject InstantiateAttack()
        {
            return Instantiate(_prefab, _transform.position + (_transform.forward * 0.5f) + _transform.up,
                _transform.rotation);
        }
    }
}