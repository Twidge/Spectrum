using UnityEngine;
using System.Collections;

public class MovingPlatformScript : MonoBehaviour
{
	public Transform startPoint;
	public Transform goalPoint;

	// Use this for initialization
	void Start ()
	{
		this.transform.position = new Vector3 (startPoint.position.x + (Spectrum.spectrumValue * (goalPoint.position.x - startPoint.position.x)), startPoint.position.y + (Spectrum.spectrumValue * (goalPoint.position.y - startPoint.position.y)), startPoint.position.z + (Spectrum.spectrumValue * (goalPoint.position.z - startPoint.position.z)));
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update position of platform while spectrum is changing
		
		if(Spectrum.mouseHeld)
		{
			this.transform.position = new Vector3 (startPoint.position.x + (Spectrum.spectrumValue * (goalPoint.position.x - startPoint.position.x)), startPoint.position.y + (Spectrum.spectrumValue * (goalPoint.position.y - startPoint.position.y)), startPoint.position.z + (Spectrum.spectrumValue * (goalPoint.position.z - startPoint.position.z)));
		}
	}
}
