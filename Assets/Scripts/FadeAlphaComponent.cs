using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAlphaComponent : MonoBehaviour
{
	public float fadeWaitTime = 2f;
	public float fadeTime = 2f;
	public bool isFadeIn = false;

	private Image fadeImage;
	private Color startColor;
	private Color endColor;

	private void Awake()
	{
		fadeImage = GetComponent<Image>();
	}

	private void Start()
	{
		if (isFadeIn)
			StartCoroutine(FadeIn());
		else
			StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		if (fadeImage == null) yield break;

		yield return new WaitForSeconds(fadeWaitTime);

		float elapsedTime = 0;
		startColor = fadeImage.color;
		endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

		while (elapsedTime < fadeTime)
		{
			fadeImage.color = Color.Lerp(startColor, endColor, (elapsedTime / fadeTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		fadeImage.color = endColor;
	}

	private IEnumerator FadeIn()
	{
		if (fadeImage == null) yield break;

		yield return new WaitForSeconds(fadeWaitTime);

		float elapsedTime = 0;
		startColor = fadeImage.color;
		endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

		while (elapsedTime < fadeTime)
		{
			fadeImage.color = Color.Lerp(startColor, endColor, (elapsedTime / fadeTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		fadeImage.color = endColor;
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
