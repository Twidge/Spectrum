using UnityEngine;
using System.Collections;

public class LaunchPadScript : MonoBehaviour
{
	public Transform wall;
	public Transform launchTarget;
	public float launchForce;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.GetComponent<Renderer>().material.SetColor ("_PadColour", wall.GetComponent<Spectrum>().wallColour);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Vector3 directionOfTravel = launchTarget.transform.position - this.transform.position;
		
		collision.collider.attachedRigidbody.AddForce (launchForce * directionOfTravel);
	}
}
