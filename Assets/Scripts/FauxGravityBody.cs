using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    public FauxGravityAttractor attractor;
    private Transform _myTransform;
    
    // Start is called before the first frame update
    private void Start()
    {
        attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        _myTransform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        attractor.Attract(_myTransform);
    }
}
