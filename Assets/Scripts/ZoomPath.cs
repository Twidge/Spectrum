using UnityEngine;
using System.Collections;

public class ZoomPath : MonoBehaviour
{
	public Transform wall;
	public Transform player;
	public float lowerBound;
	public float upperBound;
	public float adjustment;
	public ParticleSystem edgeOne;
	public ParticleSystem edgeTwo;
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log (transform.GetComponent<Renderer>().material.color);
		lowerBound = Mathf.Max(0.0f, wall.GetComponent<Spectrum>().GetSpecValueFromColour(transform.GetComponent<Renderer>().material.color) - adjustment);
		upperBound = Mathf.Min(1.0f, wall.GetComponent<Spectrum>().GetSpecValueFromColour(transform.GetComponent<Renderer>().material.color) + adjustment);
		
		edgeOne.enableEmission = false;
		edgeTwo.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float comparisonValue = Spectrum.spectrumValue;
		
		if(comparisonValue >= lowerBound && comparisonValue <= upperBound)
		{
			transform.GetComponent<Renderer>().material.SetFloat ("_ZoomMode", 1.0f);
			transform.GetComponent<Collider>().enabled = true;
			
			edgeOne.enableEmission = true;
			edgeTwo.enableEmission = true;
			
			this.tag = "ZoomPath";
		}
		
		else
		{
			transform.GetComponent<Renderer>().material.SetFloat ("_ZoomMode", 0.0f);
			transform.GetComponent<Collider>().enabled = false;
			
			edgeOne.enableEmission = false;
			edgeTwo.enableEmission = false;
			
			this.tag = "Untagged";
		}
	}
}
