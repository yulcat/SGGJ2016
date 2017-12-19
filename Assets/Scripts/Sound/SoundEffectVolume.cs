using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectVolume : MonoBehaviour
{
    AudioSource source;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        VolumeControl.AddSE(source);
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        VolumeControl.RemoveSE(source);
    }
}