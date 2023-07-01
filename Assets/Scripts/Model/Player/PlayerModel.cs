using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Model.Player
{
    /// <summary>
    /// <c>PlayerModel</c> manages game related functionalities for the player game object in unity.
    /// That includes movement, abilities, resources and cooldowns.
    /// </summary>
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
        public Cooldown GlobalCooldown => _globalCooldown;

        private readonly Cooldown _blockMovement = new();

        private int _attributePoints = 1;
        private int _intelligence = 1;
        private int _agility = 1;
        private int _strength = 1;
        private int AttributePoints => _attributePoints;
        private int Intelligence => _intelligence;
        private int Agility => _agility;
        private int Strength => _strength;

        public float PhysicalDamageModificator() => 1f + Strength / 100f;
        public float MagicDamageModificator() => 1f + Intelligence / 100f;
        public float SpeedModificator() => 1f + Agility / 100f;

        private bool HasSkillPoints()
        {
            return AttributePoints > 0;
        }

        private bool IncreaseAttribute(Action<int> attribute)
        {
            if (!HasSkillPoints()) return false;
            attribute(1);
            _attributePoints -= 1;
            return true;
        }

        public bool IncreaseStrength() => IncreaseAttribute((i) => _strength += i);
        public bool IncreaseAgility() => IncreaseAttribute((i) => _agility += i);
        public bool IncreaseIntelligence() => IncreaseAttribute((i) => _intelligence += i);


        /// <summary>
        /// Constructor that initializes a <c>PlayerModel</c> with a given <c>baseSpeed</c>.
        /// Also sets the sprinting speed and instantiates the respective resources (see <see cref="Model.Resource"/>).
        /// </summary>
        /// <param name="baseSpeed">walking speed</param>
        public PlayerModel(float baseSpeed)
        {
            _baseSpeed = baseSpeed;
            _sprintSpeed = _baseSpeed * 2f;
            _speed = _baseSpeed;
            Health = new Resource(1f);
            Stamina = new Resource(3f);
            Mana = new Resource(2f);
        }

        /// <summary>
        /// <c>GlobalCooldownActive</c> is a global cooldown that prevents the use of all abilities while active.
        /// Internally calls the <see cref="Cooldown.IsCooldownActive"/> method.
        /// </summary>
        /// <returns>
        /// True if end of global cooldown is not yet reached; otherwise, flase.
        /// </returns>
        public bool GlobalCooldownActive()
        {
            return _globalCooldown.IsCooldownActive();
        }

        /// <summary>
        /// <c>UseAbility</c> lets the player perform a specific ability animation and triggers its cooldown (see <see cref="Cooldown.Apply(float)"/>) if no global cooldown is active.
        /// </summary>
        /// <param name="ability">
        /// Ability implementing the <see cref="IAbility{T}"/> interface, where T must be of type <see cref="PlayerModel"/>
        /// </param>
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

        /// <summary>
        /// <c>Speed</c> property that allows to get and set the current speed of the player.
        /// If a new value is set while the player is sprinting, the <see cref="SprintedFor(float)"/> method is called with the total duration of the sprint.
        /// </summary>
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

        /// <summary>
        /// <c>CanSprint</c> is linked to the <c>Stamina</c> resource and <see cref="MovementBlocked"/> method and indicates whether the player can sprint.
        /// </summary>
        /// <returns>
        /// True if the <c>Stamina</c> resource is not empty and movement is not blocked; otherwise, false.
        /// </returns>
        public bool CanSprint()
        {
            return !Stamina.Empty() && !MovementBlocked();
        }

        /// <summary>
        /// <c>IsAlive</c> is linked to the <c>Health</c> resource and indicates whether the player is still alive.
        /// </summary>
        /// <returns>
        /// True if <c>Health</c> resource is not empty; otherwise, false.
        /// </returns>
        public bool IsAlive => !Health.Empty();

        /// <summary>
        /// <c>Sprint</c> lets the player sprint if he has enough <c>Stamina</c>, stay put if his movement is blocked or walk otherwise.
        /// </summary>
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

        /// <summary>
        /// <c>Stay</c> lets the player stay put by setting its <c>Speed</c> property to 0.
        /// </summary>
        public void Stay()
        {
            Speed = 0f;
            _isSprinting = false;
        }

        /// <summary>
        /// <c>Walk</c> lets the player walk if his movement is not blocked, or otherwise stay put.
        /// </summary>
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

        /// <summary>
        /// <c>BlockMovement</c> triggers a movement cooldown (see <see cref="Cooldown.Apply(float)"/>) with a given duration.
        /// </summary>
        /// <param name="duration">amount of time the player movement will be blocked</param>
        public void BlockMovement(float duration)
        {
            _blockMovement.Apply(duration);
        }

        /// <summary>
        /// <c>MovementBlocked</c> is a cooldown that prevents the player from moving while casting an ability.
        /// Internally calls the <see cref="Cooldown.IsCooldownActive"/> method.
        /// </summary>
        /// <returns>True if end of cooldown is not yet reached; otherwise, flase.</returns>
        public bool MovementBlocked()
        {
            return _blockMovement.IsCooldownActive();
        }

        /// <summary>
        /// <c>TakeDamage</c> deals the specified damage to the player by subtracting it from the player's <c>Health</c> resource.
        /// </summary>
        /// <param name="damage">amount of <c>Health</c> to be substracted</param>
        /// <returns>True if the player still has some <c>Health</c> left after taking damage; otherwise, false.</returns>
        public bool TakeDamage(float damage)
        {
            Health.Value -= damage;
            return !Health.Empty();
        }

        /// <summary>
        /// <c>SprintedFor</c> depletes the player's <c>Stamina</c> resource according to the given sprint duration and the predefined <c>StaminaDrain</c> constant.
        /// </summary>
        /// <param name="duration">amount of time the player has sprinted</param>
        private void SprintedFor(float duration)
        {
            Stamina.Value -= StaminaDrain * duration;
            _startedSprintAt = Time.time;
        }

        /// <summary>
        /// <c>Regenerate</c> regenerates the player's <c>Stamina</c>, <c>Health</c>, and <c>Mana</c> resources by calling their <see cref="Model.Resource.Regenerate(float)"/> method for a given duration.
        /// </summary>
        /// <param name="duration">amount of time the resources should regenerate</param>
        public void Regenerate(float duration)
        {
            Stamina.Regenerate(duration);
            Health.Regenerate(duration);
            Mana.Regenerate(duration);
        }
    }
}