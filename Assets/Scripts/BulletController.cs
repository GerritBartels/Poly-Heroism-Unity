using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody RB;

    [SerializeField] private float bulletSpeed = 10f;
    private float _bulletlifespan = 4f;
    private Rigidbody _rigidbody;
    private Vector3 _direction;

    [SerializeField] private LayerMask layersToHit;

    private void Start()
    {
        var mousePos = Input.mousePosition;
        // mousePos.z = Camera.main.nearClipPlane + 1;
        // mousePos = Camera.main.ScreenToWorldPoint((mousePos));
        //var rigidbodyPlayer = GameObject.Find("Player").GetComponent<Rigidbody>();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hitData, 50, layersToHit))
        {
            mousePos = hitData.point;
            print("hit: " + mousePos);
        }

        _rigidbody = GetComponent<Rigidbody>();
        var position = _rigidbody.position;
        _direction = (mousePos - position).normalized;
        print(mousePos);
    }

    public void Update()
    {
        // SHOOT BULLET
        transform.Translate(_direction * (bulletSpeed * Time.deltaTime));

        // DESTROY - if certain height is reached 
        //Destroy(this.gameObject, _bulletlifespan);
    }
}