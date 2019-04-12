using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleCreditsComponent : MonoBehaviour
{

	private Renderer thisRenderer;

	private void Start()
	{
		ToggleRenderer(false);
	}

	private void OnEnable()
	{
		Missive.AddListener<CreditsMissive>(OnToggleCredits);
		SceneManager.activeSceneChanged += ChangedActiveScene;
	}

	private void OnDisable()
	{
		Missive.RemoveListener<CreditsMissive>(OnToggleCredits);
		SceneManager.activeSceneChanged -= ChangedActiveScene;
	}

	private void ChangedActiveScene(Scene current, Scene next)
	{
		ToggleRenderer(false);
	}

	private void OnToggleCredits(CreditsMissive missive)
	{
		ToggleRenderer(!thisRenderer.enabled);
	}

	private void ToggleRenderer(bool state)
	{
		if (thisRenderer == null)
			thisRenderer = GetComponent<Renderer>();

		if (thisRenderer == null) return;

		thisRenderer.enabled = state;
	}
}
