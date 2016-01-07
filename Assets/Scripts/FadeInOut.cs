using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour
{
	public static float fadeTime = 2f;
	public bool levelEnd = false;
	private bool fadeOutPlaying = false;

	void Start ()
	{
		this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
		
		StartCoroutine(FadeSceneIn());
	}
	
	void Update ()
	{
		if(levelEnd && !fadeOutPlaying)
		{
			fadeOutPlaying = true;
			
			StartCoroutine(FadeSceneOut());
		}
	}
	
	IEnumerator FadeSceneIn()
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