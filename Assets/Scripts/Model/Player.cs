using Model.Abilities;
using UnityEngine;

namespace Model
{
    public class Player
    {
        public Resource Live { get; }

        public Resource Stamina { get; }

        public Resource Mana { get; }

        private const float StaminaDrain = 20f;
        private readonly float _baseSpeed;
        private readonly float _sprintSpeed;

        private float _speed;
        private float _startedSprintAt;
        private bool _isSprinting = false;
        
        private float _globalCooldownEnd = 0f;

        public Player(float baseSpeed)
        {
            _baseSpeed = baseSpeed;
            _sprintSpeed = _baseSpeed * 1.5f;
            _speed = _baseSpeed;
            Live = new Resource(1f);
            Stamina = new Resource(3f);
            Mana = new Resource(2f);
        }

        public bool GlobalCooldownActive()
        {
            return Time.time < _globalCooldownEnd;
        }

        public void UseAbility(IAbility ability)
        {
            if (!GlobalCooldownActive())
            {
                if (ability.Use(this))
                {
                    _globalCooldownEnd = Time.time + ability.GlobalCooldown;
                }
            }
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

        public void Sprint()
        {
            if (CanSprint())
            {
                if (!_isSprinting)
                {
                    _startedSprintAt = Time.time;
                    _isSprinting = true;
                }
                Speed = _sprintSpeed;
            }
            else
            {
                Walk();
            }
        }

        public void Walk()
        {
            Speed = _baseSpeed;
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