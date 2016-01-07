using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TextFade : MonoBehaviour
{
	public float fadeInTime;
	public static bool fadeInDone = false;
	public static bool fadeOutPlaying = false;
	private float xAdj, yAdj, zAdj, xMultAdj, yMultAdj, zMultAdj;
	
	void Start ()
	{
		if(!fadeInDone)
		{
			UnityEngine.Random.seed = DateTime.Now.Millisecond * DateTime.Now.Second;
			
			this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
			
			StartCoroutine(TextFadeIn());
			
			xAdj = UnityEngine.Random.value;
			yAdj = UnityEngine.Random.value;
			zAdj = UnityEngine.Random.value;
			xMultAdj = UnityEngine.Random.Range(0.5f, 2f);
			yMultAdj = UnityEngine.Random.Range(0.5f, 2f);
			zMultAdj = UnityEngine.Random.Range(0.5f, 2f);
		}
	}
	
	void Update ()
	{
		if(fadeInDone && !fadeOutPlaying)
		{
			fadeOutPlaying = true;
			
			StartCoroutine(TextFadeOut());
		}
	}
	
	IEnumerator TextFadeIn()
	{
		for(float f = fadeInTime; f >= 0; f -= Time.deltaTime)
		{
			Color foo = this.transform.GetComponent<Text>().color;
			
			foo.r = Mathf.Sin(xMultAdj* (f + xAdj));
			foo.g = Mathf.Cos(yMultAdj* (f + yAdj));
			foo.b = Mathf.Sin(zMultAdj* (f + zAdj));
			foo.a = (fadeInTime - f) / fadeInTime;
			
			this.transform.GetComponent<Text>().color = foo;
			
			yield return null;
		}
		
		fadeInDone = true;
	}
	
	IEnumerator TextFadeOut()
	{
		// PlayerControl.canMove = false;
		
		for(float f = 0f; f <= FadeInOut.fadeTime; f+= Time.deltaTime)
		{
			Color foo = this.transform.GetComponent<Text>().color;
			
			foo.r = Mathf.Sin(xMultAdj* (f + xAdj));
			foo.g = Mathf.Cos(yMultAdj* (f + yAdj));
			foo.b = Mathf.Sin(zMultAdj* (f + zAdj));
			foo.a = (FadeInOut.fadeTime - f) / FadeInOut.fadeTime;
			
			this.transform.GetComponent<Text>().color = foo;
			
			yield return null;
		}
		
		MenuFadeInOut.fadeInReady = true;
	}
}