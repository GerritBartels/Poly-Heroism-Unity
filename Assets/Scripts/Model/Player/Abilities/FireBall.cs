using UnityEngine;

namespace Model.Player.Abilities
{
    public class FireBall : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;
        private readonly Animator _animator;

        public FireBall(Transform transform, GameObject prefab, Animator animator) : 
            base(cooldown: 0f, globalCooldown: 0.5f, resourceCost: 10f, blockMovementFor: 0.5f)
        {
            _transform = transform;
            _prefab = prefab;
            _animator = animator;
        }

        public override void PerformAbility()
        {
            Debug.Log("Instantiate actual fireball ability");
            Instantiate(_prefab, _transform.position + (_transform.forward * 1f) + _transform.up, _transform.rotation);
        }

        protected override bool TriggerAnimation(PlayerModel player)
        {
            // TODO: add actual animation + trigger and call animator here 
            //Debug.Log("Triggered fireball animation");
            Instantiate(_prefab, _transform.position + (_transform.forward * 1f) + _transform.up, _transform.rotation);
            return true;
        }

        protected override Resource GetResource(PlayerModel player)
        {
            return player.Mana;
        }
    }
}