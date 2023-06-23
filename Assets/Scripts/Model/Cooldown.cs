using UnityEngine;

namespace Model
{
    public class Cooldown
    {
        private float _cooldownEnd = 0f;

        public bool IsCooldownActive()
        {
            return Time.time < _cooldownEnd;
        }

        public void Apply(float duration)
        {
            _cooldownEnd = Time.time + duration;
        }

        public float RemainingTime()
        {
            var dif = _cooldownEnd - Time.time;
            return dif < 0f ? 0f : dif;
        }
    }
}