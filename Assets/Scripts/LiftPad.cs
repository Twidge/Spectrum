using UnityEngine;
using System.Collections;

public class LiftPad : MonoBehaviour
{
	public Transform wall;
	public Transform player;
	
	// Spectrum variables
	
	public float lowerBound;
	public float upperBound;
	public float adjustment;
	
	// Maximum lift speed
	
	public float maxLiftSpeed;
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Bounds of lift shaft mesh are: " + transform.Find("Lift Shaft").transform.GetComponent<MeshRenderer>().bounds);
	
		lowerBound = Mathf.Max(0.0f, wall.GetComponent<Spectrum>().GetSpecValueFromColour(transform.GetComponent<Renderer>().material.color) - adjustment);
		upperBound = Mathf.Min(1.0f, wall.GetComponent<Spectrum>().GetSpecValueFromColour(transform.GetComponent<Renderer>().material.color) + adjustment);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float comparisonValue = Spectrum.spectrumValue;
		
		if(comparisonValue >= lowerBound && comparisonValue <= upperBound)
		{
			transform.GetComponent<Renderer>().material.SetFloat ("_UseWhite", 1.0f);
			transform.GetComponent<Collider>().enabled = true;
			
			if(transform.Find("Lift Shaft").transform.GetComponent<MeshRenderer>().bounds.Contains(player.transform.position))
			{
				Debug.Log ("Code executing.");
				
				if(player.transform.GetComponent<Rigidbody>().velocity.magnitude <= maxLiftSpeed)
				{
					player.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 9.8f * 1.1f);
				}
			}
			
			this.transform.Find("Particles").GetComponent<ParticleSystem>().gravityModifier = -1;
		}
		
		else
		{
			transform.GetComponent<Renderer>().material.SetFloat ("_UseWhite", 0.0f);
			transform.GetComponent<Collider>().enabled = false;
			
			this.transform.Find("Particles").GetComponent<ParticleSystem>().gravityModifier = 0;
		}
	}
}

