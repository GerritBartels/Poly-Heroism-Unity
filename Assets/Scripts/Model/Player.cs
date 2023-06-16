using UnityEngine;

namespace Model
{
    public class Player
    {
        public float Live { get; private set; } = 100;

        private const float LiveRegenerationRate = 1f;

        public float Stamina { get; private set; } = 100;

        private const float StaminaRegenerationRate = 3f;
        private const float StaminaDrain = 20f;

        private const float BaseSpeed = 10f;
        private const float SprintSpeed = 25f;

        private float _speed = BaseSpeed;

        private bool _isSprinting = false;

        private float _startedSprintAt = 0f;

        public bool IsAlive => Live > 0;

        public float Speed
        {
            get => _speed;
            private set
            {
                _speed = value;
                if (_isSprinting && (Time.time - _startedSprintAt) > 1f)
                {
                    SprintedFor(Time.time - _startedSprintAt);
                }
            }
        }


        public void Sprint()
        {
            if (CanSprint())
            {
                if (!_isSprinting)
                {
                    _startedSprintAt = Time.time;
                }

                Speed = SprintSpeed;
                _isSprinting = true;
            }
            else
            {
                Walk();
            }
        }

        public void Walk()
        {
            Speed = BaseSpeed;
            _isSprinting = false;
        }

        public bool TakeDamage(float damage)
        {
            Live -= damage;
            if (!(Live <= 0)) return true;
            Live = 0;
            return false;
        }

        private void SprintedFor(float duration)
        {
            var stamina = Stamina - StaminaDrain * duration;
            Stamina = (int)(stamina > 0 ? stamina : 0);
            _startedSprintAt = Time.time;
        }

        public bool CanSprint()
        {
            return Stamina > StaminaDrain;
        }

        public void Regenerate(float duration)
        {
            var stamina = Stamina + StaminaRegenerationRate * duration;
            var live = Live + LiveRegenerationRate * duration;
            Stamina = (stamina > 100f ? 100f : stamina);
            Live = (live > 100f ? 100f : live);
        }
    }
}