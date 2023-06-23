using UnityEngine;

namespace Model.Player.Abilities
{
    public class FireBall : AbilityBase
    {
        private readonly Transform _transform;
        private readonly GameObject _prefab;

        public FireBall(Transform transform, GameObject prefab) : base(0f, 0.5f, 10f, 0.5f)
        {
            _transform = transform;
            _prefab = prefab;
        }

        protected override bool PerformAbility(PlayerModel player)
        {
            Instantiate(_prefab, _transform.position + (_transform.forward * 1f), _transform.rotation);
            return true;
        }

        protected override Resource GetResource(PlayerModel player)
        {
            return player.Mana;
        }
    }
}