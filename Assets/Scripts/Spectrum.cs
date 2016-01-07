using UnityEngine;
using System.Collections;

public class Spectrum : MonoBehaviour
{
	public Color wallColour = new Color(1f, 0f, 0f, 0.5f);
	public static float spectrumValue = 0f;
	
	private static float cycleSpeed = 0.001017f;  // The speed at which the player cycles through the spectrum
	
	public static bool mouseHeld = false;

	void Start ()
	{
		spectrumValue = 0f;
		// PlayerControl.canMove = false;
		
		UpdateColour(spectrumValue);
	}
	
	void Update ()
	{
		// On left mouse button click/hold, changes wall colour down through spectrum (red --> blue)
		
		if(Input.GetMouseButton(0) && !(Input.GetMouseButton (1)))
		{
			if(spectrumValue <= 1f - cycleSpeed)
			{
				spectrumValue += cycleSpeed;
			}
			
			// Correct for overshoot
			
			if(spectrumValue > 1f)
			{
				spectrumValue = 1f;
			}
			
			mouseHeld = true;
		}
		
		// On right mouse button click/hold, changes wall colour up through spectrum (blue --> red)
		
		else if(Input.GetMouseButton(1) && !(Input.GetMouseButton (0)))
		{
			if(spectrumValue >= cycleSpeed)
			{
				spectrumValue -= cycleSpeed;
			}
			
			// Correct for undershoot
			
			if(spectrumValue < 0f)
			{
				spectrumValue = 0f;
			}
			
			mouseHeld = true;
		}
		
		else
		{
			mouseHeld = false;
		}
		
		// ONLY if a mouse button is being held, changes colour of wall (prevents unnecessary updating of colour)
		
		if(mouseHeld)
		{
			UpdateColour(spectrumValue);
		}
	}
	
	void UpdateColour(float x)
	{
		if(x == 0f)
		{
			wallColour = new Color (1f, 0f, 0f, 1f);
		}
		
		else if(x <= 0.25f)
		{
			wallColour = new Color(1f, x * 4f, 0f, 1f);
		}
		
		else if(x <= 0.5f)
		{
			wallColour = new Color (1f - (4f * (x - 0.25f)), 1f, 0f, 1f);
		}
		
		else if(x <= 0.75f)
		{
			wallColour = new Color (0f, 1f, (4f * (x - 0.5f)), 1f);
		}
		
		else if(x <= 1f)
		{
			wallColour = new Color (0f, 1f - (4f * (x - 0.75f)), 1f, 1f);
			
			// Scaling of x by 0.5 to account for the fact x varies between 0.5 and 1 here
		}
		
		else
		{
			wallColour = new Color (0f, 0f, 1f, 0.5f);
		}
		
		this.transform.GetComponent<Renderer>().material.color = wallColour;
	}
	
	public float GetSpecValueFromColour(Color colour)
	{
		// Assumes the colour is in the correct format!
		
		if(colour.r == 1f && colour.g == 0f)
		{
			return 0f;
		}
		
		else if(colour.r == 1f && colour.g > 0f)
		{
			return (colour.g / 4f);
		}
		
		else if(colour.r > 1f && colour.g == 1f)
		{
			return (((1f - colour.r) / 4f) + 0.25f);
		}
		
		else if(colour.r == 0f && colour.g == 1f && colour.b == 0f)
		{
			return 0.5f;
		}
		
		else if(colour.g == 1f && colour.b > 0f)
		{
			return ((colour.b / 4f) + 0.5f);
		}
		
		else if(colour.g > 1f && colour.b == 1f)
		{
			return (((1f - colour.g) / 4f) + 0.75f);
		}
		
		else
		{
			return 1f;
		}
	}
}
