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

        private Rigidbody _rigidbody;

        public Player PlayerModel { get; }

        private const float RegenerationDelay = 1f;
        
        private Animator _animator;

        private RangedAttack _rangedAttack;
        private ScatterShot _scatterShot;
        private MeleeAttack _meleeAttack;

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
            
            _animator = GetComponent<Animator>();

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

            //sprint or walk
            if (Input.GetKey(KeyCode.LeftShift))
            {
                PlayerModel.Sprint();
            }
            else
            {
                PlayerModel.Walk();
            }
            
            // Pass the player's movement direction to the animator
            // Might make sense to inlcude a sideways movement animation as well for a and d
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) 
            {
                _animator.SetFloat("playerSpeed", PlayerModel.Speed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _animator.SetFloat("playerSpeed", -PlayerModel.Speed);
            }
            else
            {
                _animator.SetFloat("playerSpeed", 0);
            }
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