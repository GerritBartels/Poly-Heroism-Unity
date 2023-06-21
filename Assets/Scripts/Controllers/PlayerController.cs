using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Model;
using Model.Abilities;

namespace Controllers
{
    /// <summary>
    /// <c>PlayerController</c> defines control for the player game object within unity.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private Vector3 _moveDirection;
        private float _mouseX;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject meleePrefab;

        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float rotationSpeed = 4000f;

        private Rigidbody _rigidbody;

        public Player PlayerModel { get; }

        private const float RegenerationDelay = 1f;

        private RangedAttack _rangedAttack;
        private ScatterShot _scatterShot;
        private MeleeAttack _meleeAttack;

        /// <summary>
        /// Constructor that initializes a <c>PlayerController</c> by instantiating a new <see cref="Player"/> with a given <c>baseSpeed</c>.
        /// </summary>
        private PlayerController()
        {
            PlayerModel = new Player(baseSpeed);
        }

        private void Start()
        {
            _rangedAttack = new RangedAttack(transform, bulletPrefab);
            _scatterShot = new ScatterShot(transform, bulletPrefab);
            _meleeAttack = new MeleeAttack(transform, meleePrefab);

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            StartCoroutine(Regeneration());
        }

        public void Update()
        {
            // Attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shot();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ScatterShot();
            }

            // Sprint or walk
            if (Input.GetKey(KeyCode.LeftShift))
            {
                PlayerModel.Sprint();
            }
            else
            {
                PlayerModel.Walk();
            }
        }

        public void LateUpdate()
        {
            // Rotate player
            _mouseX = Input.GetAxis("Mouse X");
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * _mouseX * rotationSpeed);
        }

        /// <summary>
        /// <c>Shot</c> calls the <see cref="Player.UseAbility(IAbility)"/> method with the <see cref="Model.Abilities.RangedAttack"/> ability.
        /// </summary>
        private void Shot()
        {
            PlayerModel.UseAbility(_rangedAttack);
        }

        /// <summary>
        /// <c>Attack</c> calls the <see cref="Player.UseAbility(IAbility)"/> method with the <see cref="Model.Abilities.MeleeAttack"/> ability.
        /// </summary>
        private void Attack()
        {
            PlayerModel.UseAbility(_meleeAttack);
        }

        /// <summary>
        /// <c>ScatterShot</c> calls the <see cref="Player.UseAbility(IAbility)"/> method with the <see cref="Model.Abilities.ScatterShot"/> ability.
        /// </summary>
        private void ScatterShot()
        {
            PlayerModel.UseAbility(_scatterShot);
        }

        public void FixedUpdate()
        {
            // Move player
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            _rigidbody.MovePosition(_rigidbody.position +
                                    transform.TransformDirection(_moveDirection) *
                                    (PlayerModel.Speed * Time.deltaTime));
        }

        /// <summary>
        /// <c>Damage</c> calls the <see cref="Player.TakeDamage"/> method and destroys the player game object upon death.
        /// </summary>
        /// <param name="damage">the amount of damage taken</param>
        public void Damage(float damage)
        {
            if (!PlayerModel.TakeDamage(damage))
            {
                // Death
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// <c>Regeneration</c> calls the <see cref="Player.Regenerate(float)"/> method with a given <c>RegenerationDelay</c> while the Player is alive.
        /// </summary>
        /// <returns>
        /// <see cref="WaitForSeconds"/> delay if player is alive; otherwise, <c>null</c>.
        /// </returns>
        protected virtual IEnumerator Regeneration()
        {
            while (PlayerModel.IsAlive)
            {
                PlayerModel.Regenerate(RegenerationDelay);
                yield return new WaitForSeconds(RegenerationDelay);
            }

            yield return null;
        }
    }
}