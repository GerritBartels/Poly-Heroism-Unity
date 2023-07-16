using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    public float gravity = -10f;
    public void Attract(Transform body)
    {
        var gravityUp = (body.position - transform.position).normalized;
        var localUp = body.up;
        
        body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);

        var rotation = body.rotation;
        var targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * rotation;
        rotation = Quaternion.Slerp(rotation, targetRotation, 50f * Time.deltaTime);
        body.rotation = rotation;
    }
}
