using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Model;
using Model.Player;
using Model.Player.Abilities;

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
        [SerializeField] private GameObject fireBallPrefab;


        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float rotationSpeed = 4000f;

        private Rigidbody _rigidbody;

        public PlayerModel PlayerModel { get; }

        private const float RegenerationDelay = 1f;

        private RangedAttack _rangedAttack;
        private ScatterShot _scatterShot;
        private MeleeAttack _meleeAttack;
        private FireBall _fireBall;
        private BulletTime _bulletTime;

        /// <summary>
        /// Constructor that initializes a <c>PlayerController</c> by instantiating a new <see cref="Model.Player.PlayerModel"/> with a given <c>baseSpeed</c>.
        /// </summary>
        private PlayerController()
        {
            PlayerModel = new PlayerModel(baseSpeed);
        }

        private void Start()
        {
            // Instantiate abilites
            _rangedAttack = new RangedAttack(transform, bulletPrefab);
            _scatterShot = new ScatterShot(transform, bulletPrefab);
            _meleeAttack = new MeleeAttack(transform, meleePrefab);
            _fireBall = new FireBall(transform, fireBallPrefab);
            _bulletTime = new BulletTime(this);

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            StartCoroutine(Regeneration());
        }

        public void Update()
        {
            // Attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlayerModel.UseAbility(_rangedAttack);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                PlayerModel.UseAbility(_meleeAttack);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerModel.UseAbility(_scatterShot);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerModel.UseAbility(_fireBall);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerModel.UseAbility(_bulletTime);
            }

            //sprint or walk
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

        public void FixedUpdate()
        {
            // Move player
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            _rigidbody.MovePosition(_rigidbody.position +
                                    transform.TransformDirection(_moveDirection) *
                                    (PlayerModel.Speed * Time.deltaTime));
        }

        /// <summary>
        /// <c>Damage</c> calls the <see cref="PlayerModel.TakeDamage"/> method and destroys the player game object upon death.
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
        /// <c>Regeneration</c> coroutine calls the <see cref="PlayerModel.Regenerate(float)"/> method with a given <c>RegenerationDelay</c> while the Player is alive.
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