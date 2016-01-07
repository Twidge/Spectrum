using UnityEngine;
using System.Collections;

public class ConditionalPlatform : MonoBehaviour
{
	public Transform wall;
	public Transform player;
	public float lowerBound;
	public float upperBound;
	public float proximityBound;
	public float adjustment;

	// Use this for initialization
	void Start ()
	{
		Debug.Log (transform.GetComponent<Renderer>().material.color);
		lowerBound = Mathf.Max(0.0f, wall.GetComponent<Spectrum>().GetSpecValueFromColour(transform.GetComponent<Renderer>().material.color) - adjustment);
		upperBound = Mathf.Min(1.0f, wall.GetComponent<Spectrum>().GetSpecValueFromColour(transform.GetComponent<Renderer>().material.color) + adjustment);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float comparisonValue = Spectrum.spectrumValue;
		
		if(comparisonValue >= lowerBound && comparisonValue <= upperBound)
		{
			transform.GetComponent<Renderer>().material.SetFloat ("_Proximity", 0);
			transform.GetComponent<Renderer>().material.SetFloat ("_UseWhite", 1.0f);
			transform.GetComponent<Collider>().enabled = true;
		}
		
		else if(comparisonValue >= lowerBound - proximityBound && comparisonValue < lowerBound)
		{
			float foo = (4 * (comparisonValue - (lowerBound - proximityBound))) / proximityBound;
			transform.GetComponent<Renderer>().material.SetFloat ("_UseWhite", 0.0f);
			transform.GetComponent<Renderer>().material.SetFloat ("_Proximity", foo);
			transform.GetComponent<Collider>().enabled = false;
		}
		
		else if(comparisonValue <= upperBound + proximityBound && comparisonValue > upperBound)
		{
			float foo = 4 - ((4 * (comparisonValue - upperBound)) / proximityBound);
			transform.GetComponent<Renderer>().material.SetFloat ("_UseWhite", 0.0f);
			transform.GetComponent<Renderer>().material.SetFloat ("_Proximity", foo);
			transform.GetComponent<Collider>().enabled = false;
		}
		
		else
		{
			transform.GetComponent<Renderer>().material.SetFloat ("_Proximity", 0);
			transform.GetComponent<Renderer>().material.SetFloat ("_UseWhite", 0.0f);
			transform.GetComponent<Collider>().enabled = false;
		}
	}
}
