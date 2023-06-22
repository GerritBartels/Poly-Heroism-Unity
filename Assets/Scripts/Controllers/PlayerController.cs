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
    public class PlayerController : MonoBehaviour
    {
        private Vector3 _moveDirection;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject meleePrefab;

        [SerializeField] private float baseSpeed = 5f;

        private Rigidbody _rigidbody;

        public PlayerModel PlayerModel { get; }

        private const float RegenerationDelay = 1f;

        private RangedAttack _rangedAttack;
        private ScatterShot _scatterShot;
        private MeleeAttack _meleeAttack;
        private BulletTime _bulletTime;

        private PlayerController()
        {
            PlayerModel = new PlayerModel(baseSpeed);
        }

        private void Start()
        {
            _rangedAttack = new RangedAttack(transform, bulletPrefab);
            _scatterShot = new ScatterShot(transform, bulletPrefab);
            _meleeAttack = new MeleeAttack(transform, meleePrefab);
            _bulletTime = new BulletTime(this);

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            StartCoroutine(Regeneration());
        }

        public void Update()
        {
            // attack
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

        public void FixedUpdate()
        {
            //move player
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            _rigidbody.MovePosition(_rigidbody.position +
                                    transform.TransformDirection(_moveDirection) *
                                    (PlayerModel.Speed * Time.deltaTime));
        }

        public void Damage(float damage)
        {
            if (!PlayerModel.TakeDamage(damage))
            {
                // death
                Destroy(this.gameObject);
            }
        }

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