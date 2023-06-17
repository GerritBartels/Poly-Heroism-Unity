using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBehaviour : MonoBehaviour
{
    [SerializeField] private bool autoOrient = true;
    [SerializeField] private float autoOrientSpeed = 1f;

    [SerializeField] public float gravity = 9.81f;

    private Transform _gravityTarget;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gravityTarget = GameObject.Find("Planet").GetComponent<Transform>();
    }

    public void ProcessGravity()
    {
        var diff = transform.position - _gravityTarget.position;
        _rb.AddForce(-diff.normalized * (gravity * _rb.mass));

        if (autoOrient)
        {
            AutoOrient(-diff);
        }
    }

    private void AutoOrient(Vector3 down)
    {
        var transform1 = transform;
        var rotation = transform1.rotation;

        Quaternion targetRotation = Quaternion.FromToRotation(-transform1.up, down) * rotation;
        rotation = Quaternion.Slerp(rotation, targetRotation, autoOrientSpeed * Time.deltaTime);

        transform.rotation = rotation;
    }

    void FixedUpdate()
    {
        ProcessGravity();
    }
}