using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]


public class BarkDelay : MonoBehaviour

{
    public float delayAmount;
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayDelayed(delayAmount);
    }
}
