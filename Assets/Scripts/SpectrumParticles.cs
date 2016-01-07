using UnityEngine;
using System.Collections;

public class SpectrumParticles : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.GetComponent<ParticleSystem>().startColor = this.GetComponent<Spectrum>().wallColour;
	}
}
