using UnityEngine;
using System.Collections;

public class SpectrumText : MonoBehaviour
{
	public Color wallColour = new Color(1f, 0f, 0f, 0.5f);
	public static float spectrumValue = 0f;
	
	private static float cycleSpeed = 0.002033f;  // The speed at which the player cycles through the spectrum
	
	public static bool mouseHeld = false;
	
	void Start ()
	{
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
			wallColour = new Color (1f, 0f, 0f, 0.5f);
		}
		
		else if(x <= 0.5f)
		{
			wallColour = new Color (1f - (2 * x), (2 * x), 0f, 0.5f);
		}
		
		else if(x <= 1f)
		{
			wallColour = new Color (0f, 1f - (2 * (x - 0.5f)), (2 * (x - 0.5f)), 0.5f);
			
			// Scaling of x by 0.5 to account for the fact x varies between 0.5 and 1 here
		}
		
		else
		{
			wallColour = new Color (0f, 0f, 1f, 0.5f);
		}
		
		this.transform.GetComponent<TextMesh>().color = wallColour;
	}
	
	public float GetSpecValueFromColour(Color colour)
	{
		// Assumes the colour is in the correct format!
		
		if(colour.r != 0)
		{
			return (colour.g / 2.0f);
		}
		
		else
		{
			return ((colour.b + 1f) / 2.0f);
		}
	}
}
