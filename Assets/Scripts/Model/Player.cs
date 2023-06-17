using UnityEngine;

namespace Model
{
    public class Player
    {
        public Resource Live { get; }

        public Resource Stamina { get; }

        public Resource Mana { get; }

        private const float StaminaDrain = 20f;
        private const float BaseSpeed = 10f;
        private const float SprintSpeed = BaseSpeed * 1.5f;

        private float _speed = BaseSpeed;
        private float _startedSprintAt;
        private bool _isSprinting = false;


        public Player()
        {
            Live = new Resource(1f);
            Stamina = new Resource(3f);
            Mana = new Resource(2f);
        }

        public float Speed
        {
            get => _speed;
            private set
            {
                _speed = value;
                if (_isSprinting)
                {
                    SprintedFor(Time.time - _startedSprintAt);
                }
            }
        }

        public bool CanSprint()
        {
            return !Stamina.Empty();
        }

        public bool IsAlive => !Live.Empty();

        public bool CanCast(float cost)
        {
            return Mana.Value > cost;
        }

        public bool CastFor(float cost)
        {
            if (!CanCast(cost)) return false;
            Mana.Value -= cost;
            return true;
        }

        public void Sprint()
        {
            if (CanSprint())
            {
                if (!_isSprinting)
                {
                    _startedSprintAt = Time.time;
                    _isSprinting = true;
                }

                Speed = SprintSpeed;
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
            Live.Value -= damage;
            return !Live.Empty();
        }

        private void SprintedFor(float duration)
        {
            Stamina.Value -= StaminaDrain * duration;
            _startedSprintAt = Time.time;
        }


        public void Regenerate(float duration)
        {
            Stamina.Regenerate(duration);
            Live.Regenerate(duration);
            Mana.Regenerate(duration);
        }
    }
}