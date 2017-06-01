using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class VolumeControl {
	static List<AudioSource> activeSESources = new List<AudioSource>();
	static float seVolume = 1f;
	static float bgVolume = 1f;
	public static float seVol
	{
		get
		{
			return seVolume;
		}
	}
	public static float bgVol
	{
		get
		{
			return bgVolume;
		}
	}
	public static void AddSE(AudioSource newSource)
	{
		if(activeSESources == null)
			activeSESources = new List<AudioSource>();
		if(activeSESources.Contains(newSource)) return;
		activeSESources.Add(newSource);
		newSource.volume = seVolume;
	}
	public static void RemoveSE(AudioSource sourceToRemove)
	{
		if(!activeSESources.Contains(sourceToRemove)) return;
		activeSESources.Remove(sourceToRemove);
	}
	public static void SetSEVolume(float vol)
	{
		seVolume = vol;
		activeSESources.RemoveAll(s => s==null);
		activeSESources.ForEach(se => se.volume = vol);
		SaveDataManager.data.seVolume = vol;
	}
	public static void SetBGVolume(float vol)
	{
		bgVolume = vol;
		BGM.SetVolume(vol);
		SaveDataManager.data.bgmVolume = vol;
	}

    internal static void SetBGVolumeFromLoad(float vol)
    {
        seVolume = vol;
		BGM.SetVolume(vol);
    }

    internal static void SetSEVolumeFromLoad(float vol)
    {
        bgVolume = vol;
    }
}
