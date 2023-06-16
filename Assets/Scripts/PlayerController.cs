using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Model;

public class PlayerController : MonoBehaviour
{
    private Vector3 _moveDirection;

    [SerializeField] private GameObject bulletPrefab;
    private Rigidbody _rigidbody;

    private readonly Player _playerModel;

    private const float RegenerationDelay = 1f;

    private PlayerController()
    {
        _playerModel = new Player();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(Regeneration());
    }

    public void Update()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        // SPAWN BULLET
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _playerModel.Sprint();
        }
        else
        {
            _playerModel.Walk();
        }

        _rigidbody.MovePosition(_rigidbody.position +
                                transform.TransformDirection(_moveDirection) * (_playerModel.Speed * Time.deltaTime));
        Debug.Log($"Stamina: {_playerModel.Stamina}");
        Debug.Log($"Live: {_playerModel.Live}");
    }

    public void FixedUpdate()
    {
    }

    public void Damage(float damage)
    {
        if (!_playerModel.TakeDamage(damage))
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Regeneration()
    {
        while (_playerModel.IsAlive)
        {
            _playerModel.Regenerate(RegenerationDelay);
            yield return new WaitForSeconds(RegenerationDelay);
        }

        yield return null;
    }
}