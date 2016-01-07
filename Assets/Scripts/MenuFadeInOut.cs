using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuFadeInOut : MonoBehaviour
{
	public static float fadeTime = 5f;
	public static bool fadeInReady = false;
	public static bool fadeInPlaying = false;
	public static bool levelEnd = false;
	public static bool fadeOutPlaying = false;
	
	void Start ()
	{
		this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
	}
	
	void Update ()
	{
		if(fadeInReady && !fadeInPlaying)
		{
			fadeInPlaying = true;
		
			StartCoroutine(FadeTextIn());
		}
	
		if(levelEnd && !fadeOutPlaying)
		{
			fadeOutPlaying = true;
			
			StartCoroutine(FadeTextOut());
		}
	}
	
	IEnumerator FadeTextIn()
	{
		for(float f = fadeTime; f >= 0; f -= Time.deltaTime)
		{
			Color foo = this.transform.GetComponent<Image>().color;
			
			foo.a = f / fadeTime;
			
			this.transform.GetComponent<Image>().color = foo;
			
			yield return null;
		}
		
		// PlayerControl.canMove = true;
	}
	
	IEnumerator FadeTextOut()
	{
		// PlayerControl.canMove = false;
		
		for(float f = 0f; f <= fadeTime; f += Time.deltaTime)
		{
			Color foo = this.transform.GetComponent<Image>().color;
			
			foo.a = f / fadeTime;
			
			this.transform.GetComponent<Image>().color = foo;
			
			yield return null;
		}
		
		Spectrum.spectrumValue = 0f;
		
		Application.LoadLevel (Application.loadedLevel + 1);
	}
	
	IEnumerator FadeSceneOut()
	{
		// PlayerControl.canMove = false;
		
		for(float f = 0f; f <= fadeTime; f += Time.deltaTime)
		{
			Color foo = this.transform.GetComponent<Image>().color;
			
			foo.a = f / fadeTime;
			
			this.transform.GetComponent<Image>().color = foo;
			
			yield return null;
		}
		
		Spectrum.spectrumValue = 0f;
		
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}