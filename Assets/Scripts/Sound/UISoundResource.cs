﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName="Content/SoundResource")]
public class UISoundResource : ScriptableObject {
	[System.SerializableAttribute]
	public struct AudioPair
	{
		public string name;
		public AudioClip clip;
	}
	public AudioPair[] clips;
	static UISoundResource instance;
	public static UISoundResource resource
	{
		get
		{
			if(instance == null)
			{
				instance = Resources.Load<UISoundResource>("UISoundResource");
			}
			return instance;
		}
	}
}
