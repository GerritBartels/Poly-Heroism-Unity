using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDirection;
    
    [SerializeField]
    private GameObject _bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
        // SPAWN BULLET 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(_bulletPrefab, transform.position, transform.rotation);
        }
    }
    
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
    }
}
