using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    
    [SerializeField]
    private Rigidbody RB;
    
    [SerializeField]
    private float _bulletSpeed = 10f;
    private float _bulletlifespan = 4f;

    
    void Update()
    {
        // SHOOT BULLET
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);

        // DESTROY - if certain height is reached 
        //Destroy(this.gameObject, _bulletlifespan);
    }
}
