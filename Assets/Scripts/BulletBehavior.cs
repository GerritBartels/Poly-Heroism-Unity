using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletBehavior : MonoBehaviour
{
    
    [SerializeField]
    private Rigidbody RB;
    
    [SerializeField]
    private float bulletSpeed = 10f;
    private float _bulletlifespan = 4f;


    public void Update()
    {
        // SHOOT BULLET
        transform.Translate(Vector3.forward * (bulletSpeed * Time.deltaTime));

        // DESTROY - if certain height is reached 
        //Destroy(this.gameObject, _bulletlifespan);
    }
}
