using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAlphaComponent : MonoBehaviour
{
	#region vars
	[Header("Image and Text Fade Alpha")]
	public bool isFadeIn = false;
	public GameObject hideParentGO;
	public float fadeWaitTime = 2f;
	public float fadeTime = 2f;

	private Image fadeImage;
	private Text fadeText;
	private Color startColor;
	private Color endColor;
	#endregion

	#region mono
	private void Awake()
	{
		fadeImage = GetComponent<Image>();
		fadeText = GetComponent<Text>();
	}

	private void Start()
	{
		if (hideParentGO != null)
			hideParentGO.SetActive(true);

		if (fadeImage != null)
		{
			if (isFadeIn)
				StartCoroutine(FadeImageIn());
			else
				StartCoroutine(FadeImageOut());
		}
		
		if (fadeText != null)
		{
			if (isFadeIn)
				StartCoroutine(FadeTextIn());
			else
				StartCoroutine(FadeTextOut());
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
	#endregion

	#region image
	private IEnumerator FadeImageOut()
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
		if (hideParentGO != null)
			hideParentGO.SetActive(false);
	}

	private IEnumerator FadeImageIn()
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
		if (hideParentGO != null)
			hideParentGO.SetActive(false);
	}
	#endregion

	#region text
	private IEnumerator FadeTextOut()
	{
		if (fadeText == null) yield break;
		yield return new WaitForSeconds(fadeWaitTime);

		float elapsedTime = 0;
		startColor = fadeText.color;
		endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

		while (elapsedTime < fadeTime)
		{
			fadeText.color = Color.Lerp(startColor, endColor, (elapsedTime / fadeTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		fadeText.color = endColor;
		if (hideParentGO != null)
			hideParentGO.SetActive(false);
	}

	private IEnumerator FadeTextIn()
	{
		if (fadeText == null) yield break;
		yield return new WaitForSeconds(fadeWaitTime);

		float elapsedTime = 0;
		startColor = fadeText.color;
		endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

		while (elapsedTime < fadeTime)
		{
			fadeText.color = Color.Lerp(startColor, endColor, (elapsedTime / fadeTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		fadeText.color = endColor;
		if (hideParentGO != null)
			hideParentGO.SetActive(false);
	}
	#endregion
}
