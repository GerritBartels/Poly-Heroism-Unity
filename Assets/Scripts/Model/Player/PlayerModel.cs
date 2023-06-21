using UnityEngine;

namespace Model.Player
{
    public class PlayerModel
    {
        public Resource Health { get; }

        public Resource Stamina { get; }

        public Resource Mana { get; }

        private const float StaminaDrain = 20f;
        private readonly float _baseSpeed;
        private readonly float _sprintSpeed;

        private float _speed;
        private float _startedSprintAt;
        private bool _isSprinting = false;

        private readonly Cooldown _globalCooldown = new();
        private readonly Cooldown _blockMovement = new();

        public PlayerModel(float baseSpeed)
        {
            _baseSpeed = baseSpeed;
            _sprintSpeed = _baseSpeed * 1.5f;
            _speed = _baseSpeed;
            Health = new Resource(1f);
            Stamina = new Resource(3f);
            Mana = new Resource(2f);
        }

        public bool GlobalCooldownActive()
        {
            return _globalCooldown.IsCooldownActive();
        }

        public void UseAbility(IAbility<PlayerModel> ability)
        {
            if (!GlobalCooldownActive())
            {
                if (ability.Use(this))
                {
                    _globalCooldown.Apply(ability.GlobalCooldown);
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
            return !Stamina.Empty() && !MovementBlocked();
        }

        public bool IsAlive => !Health.Empty();

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
            else if (MovementBlocked())
            {
                Stay();
            }
            else
            {
                Walk();
            }
        }

        public void Stay()
        {
            Speed = 0f;
            _isSprinting = false;
        }

        public void Walk()
        {
            if (MovementBlocked())
            {
                Stay();
            }
            else
            {
                Speed = _baseSpeed;
                _isSprinting = false;
            }
        }

        public void BlockMovement(float duration)
        {
            _blockMovement.Apply(duration);
        }

        public bool MovementBlocked()
        {
            return _blockMovement.IsCooldownActive();
        }

        public bool TakeDamage(float damage)
        {
            Health.Value -= damage;
            return !Health.Empty();
        }

        private void SprintedFor(float duration)
        {
            Stamina.Value -= StaminaDrain * duration;
            _startedSprintAt = Time.time;
        }

        public void Regenerate(float duration)
        {
            Stamina.Regenerate(duration);
            Health.Regenerate(duration);
            Mana.Regenerate(duration);
        }
    }
}