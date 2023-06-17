using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Model;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private Vector3 _moveDirection;

        [SerializeField] private GameObject bulletPrefab;
        private Rigidbody _rigidbody;

        public Player PlayerModel { get; }

        private const float RegenerationDelay = 1f;

        private PlayerController()
        {
            PlayerModel = new Player();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            StartCoroutine(Regeneration());
        }

        public void Update()
        {
            // attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shout();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ScatterShout();
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

        private void Shout()
        {
            var transform1 = transform;
            Instantiate(bulletPrefab, transform1.position + (transform1.forward * 0.5f), transform1.rotation);
        }

        private void ScatterShout()
        {
            var scatterShoutManaCost = 20;
            if (!PlayerModel.CastFor(scatterShoutManaCost)) return;
            Instantiate(
                bulletPrefab,
                transform.position + (transform.forward * 0.5f),
                transform.rotation
            );
            Instantiate(
                bulletPrefab,
                transform.position + (transform.forward * 0.5f) + (transform.right * -0.3f),
                transform.rotation * Quaternion.Euler(0f, -10f, 0f)
            );
            Instantiate(
                bulletPrefab,
                transform.position + (transform.forward * 0.5f) + (transform.right * 0.3f),
                transform.rotation * Quaternion.Euler(0f, 10f, 0f)
            );
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