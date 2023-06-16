using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletEnemyBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody RB;

    [SerializeField] private float bulletSpeed = 50f;


    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private Rigidbody _rigidbodyPlayer;


    private void Start()
    {
        _rigidbodyPlayer = GameObject.Find("Player").GetComponent<Rigidbody>();
        _rigidbody = GetComponent<Rigidbody>();
        var position = _rigidbody.position;
        var playerPosition = _rigidbodyPlayer.position;
        _direction = (playerPosition - position).normalized;
    }

    private void Update()
    {
        // SHOOT BULLET
        transform.Translate(_direction * (bulletSpeed * Time.deltaTime));

        // DESTROY - if certain height is reached 
        //Destroy(this.gameObject, _bulletlifespan);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // DESTROY ENEMY + BULLET
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Damage(10);
            Destroy(this.gameObject);
        }
    }
    
}