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
    public class PlayerController : MonoBehaviour
    {
        private Vector3 _moveDirection;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject meleePrefab;

        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private GameObject planet;

        private Rigidbody _rigidbody;

        public Player PlayerModel { get; }

        private const float RegenerationDelay = 1f;

        private RangedAttack _rangedAttack;
        private ScatterShot _scatterShot;
        private MeleeAttack _meleeAttack;

        private Camera cam;

        private PlayerController()
        {
            PlayerModel = new Player(baseSpeed);
        }

        private void Start()
        {
            cam = Camera.main;
            _rangedAttack = new RangedAttack(transform, bulletPrefab);
            _scatterShot = new ScatterShot(transform, bulletPrefab);
            _meleeAttack = new MeleeAttack(transform, meleePrefab);

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            StartCoroutine(Regeneration());
        }

        public void Update()
        {
            // attack
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

            // sprint or walk
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
            float mouseX = Input.GetAxis("Mouse X");
            //float mouseY = Input.GetAxis("Mouse Y");
            float rotationSpeed = 950f;
            float rotationY = mouseX * rotationSpeed;

            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationY);
            //planet.transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
        }

        private void Shot()
        {
            PlayerModel.UseAbility(_rangedAttack);
        }

        private void Attack()
        {
            PlayerModel.UseAbility(_meleeAttack);
        }

        private void ScatterShot()
        {
            PlayerModel.UseAbility(_scatterShot);
        }

        public void FixedUpdate()
        {
            // move player
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