using Model;
using Model.Player;
using UnityEngine;

namespace Controllers.Player.AbilityAdapters
{
    public class FireBall : AttackControllerAbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;
        private static readonly int Fireball = Animator.StringToHash("fireball");

        public FireBall(Transform transform, GameObject prefab, Animator animator) :
            base(cooldown: 0.5f, globalCooldown: 0.5f, resourceCost: 10f, blockMovementFor: 0.5f, baseDamage: 50f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        protected override bool TriggerAnimation(PlayerModel player)
        {
            _animator.SetTrigger(Fireball);
            //base.PerformAbility(player);
            return true;
        }

        protected override Resource GetResource(PlayerModel player) => player.Mana;

        protected override float DamageModifier(PlayerModel playerModel) => playerModel.MagicDamageModificator();

        protected override GameObject InstantiateAttack()
        {
            return Instantiate(_prefab, _transform.position + (_transform.forward * 0.7f) + _transform.up,
                _transform.rotation);
        }
    }
}