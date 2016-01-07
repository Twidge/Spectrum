using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
	public Transform wall;
	public Transform player;
	public float lowerBound;
	public float upperBound;
	public float adjustment;
	
	// If enabled, automaticPlacement automatically determines the position of the player when they walk through the teleporter
	
	public bool automaticPlacement;
	
	public Transform oppositePort;
	
	// Use this for initialization
	void Start ()
	{
		lowerBound = Mathf.Max(0.0f, wall.GetComponent<Spectrum>().GetSpecValueFromColour(transform.GetComponent<Renderer>().material.color) - adjustment);
		upperBound = Mathf.Min(1.0f, wall.GetComponent<Spectrum>().GetSpecValueFromColour(transform.GetComponent<Renderer>().material.color) + adjustment);
		
		Transform outPoint = this.transform.Find("OutPoint");
		
		if(automaticPlacement)
		{
			outPoint.transform.position = oppositePort.transform.position - new Vector3 (2, 0, 0);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		float comparisonValue = Spectrum.spectrumValue;
		
		if(comparisonValue >= lowerBound && comparisonValue <= upperBound)
		{
			transform.GetComponent<Renderer>().material.SetFloat ("_UseWhite", 1.0f);
			transform.GetComponent<Collider>().enabled = true;
		}
		
		else
		{
			transform.GetComponent<Renderer>().material.SetFloat ("_UseWhite", 0.0f);
			transform.GetComponent<Collider>().enabled = false;
		}
	}
}
