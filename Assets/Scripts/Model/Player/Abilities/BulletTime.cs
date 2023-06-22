using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Model.Player.Abilities
{
    public class BulletTime : IAbility<PlayerModel>
    {
        public float Cooldown => 0;
        public float GlobalCooldown => 0;
        public float ResourceCost { get; }
        public float CooldownTimeRemaining() => 0;

        private readonly float _tickSpeed = 0.1f;

        private bool _bulletTimeActive;

        private readonly MonoBehaviour _behaviour;

        public BulletTime(MonoBehaviour behaviour)
        {
            _behaviour = behaviour;
            _bulletTimeActive = false;
            ResourceCost = 20;
        }

        protected void PerformAbility()
        {
            Time.timeScale = 0.4f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            _bulletTimeActive = true;
        }

        private void Deactivate()
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;
            _bulletTimeActive = false;
        }

        public bool Use(PlayerModel playerModel)
        {
            if (!_bulletTimeActive)
            {
                PerformAbility();
                _behaviour.StartCoroutine(ManaDrain(playerModel.Mana));
                return true;
            }

            Deactivate();
            return false;
        }

        protected virtual IEnumerator ManaDrain(Resource resource)
        {
            while (_bulletTimeActive)
            {
                if (resource.Value >= ResourceCost * _tickSpeed)
                {
                    resource.Drain(ResourceCost, _tickSpeed);
                }
                else
                {
                    Deactivate();
                }

                yield return new WaitForSeconds(_tickSpeed);
            }

            yield return null;
        }
    }
}