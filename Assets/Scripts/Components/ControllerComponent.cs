using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerComponent : MonoBehaviour
{
	public AudioSource yearAudioSource;

	private void OnEnable()
	{
		Missive.AddListener<AudioMissive>(OnAudioClip);
	}

	private void OnDisable()
	{
		Missive.RemoveListener<AudioMissive>(OnAudioClip);
	}

	private void OnAudioClip(AudioMissive missive)
	{
		yearAudioSource.PlayOneShot(yearAudioSource.clip, 0.5f);
	}
}
