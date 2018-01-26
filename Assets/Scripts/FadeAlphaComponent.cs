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
	private void Awake()
	{
		fadeImage = GetComponent<Image>();
	}

	private void Start()
	{
		// TODO removed until Unity 2017.3.0f3 UI bugs are fixed
		//StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		if (fadeImage == null) yield break;

		yield return new WaitForSeconds(fadeWaitTime);

		float elapsedTime = 0;
		startColor = fadeImage.material.color;

		while (elapsedTime < fadeTime)
		{
			fadeImage.material.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0f), (elapsedTime / fadeTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		fadeImage.material.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
	}

	private IEnumerator FadeIn()
	{
		if (fadeImage == null) yield break;

		yield return new WaitForSeconds(fadeWaitTime);

		float elapsedTime = 0;
		fadeImage.material.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
		startColor = fadeImage.material.color;

		while (elapsedTime < fadeTime)
		{
			fadeImage.material.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 1f), (elapsedTime / fadeTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		fadeImage.material.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
