using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody Rb;

    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;

	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Rb.AddForce(0, 0, forwardForce*Time.deltaTime);

        if (Input.GetKey("q"))
        {
            Rb.AddForce(-sidewaysForce * Time.deltaTime, 0,0);
        }
        if (Input.GetKey("w"))
        {
            Rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0);
        }
    }
}
