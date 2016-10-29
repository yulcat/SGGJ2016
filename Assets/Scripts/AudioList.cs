using UnityEngine;
using System.Linq;

public class AudioList : MonoBehaviour {
	[System.Serializable]
	public struct ClipPack
	{
		public string name;
		public AudioClip[] clips;
	}
	public ClipPack[] packs;
	AudioSource source;
	void Awake()
	{
		source = GetComponent<AudioSource>();
	}
	public void Play(string name)
	{
		var pack = packs.First(p => p.name == name);
		var clip = pack.clips.OrderBy(c => Random.Range(0f,1f)).First();
		source.clip = clip;
		source.Play();
	}
}
