using UnityEngine;
using System.Linq;

public class AudioList : MonoBehaviour
{
    public bool playOnAwake = false;

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
        VolumeControl.AddSE(source);
    }

    void OnEnable()
    {
        if (playOnAwake)
        {
            source.clip = packs.First().clips.OrderBy(c => Random.Range(0f, 1f)).First();
            source.Play();
        }
    }

    public void Play(string name)
    {
        var pack = packs.First(p => p.name == name);
        var clip = pack.clips.OrderBy(c => Random.Range(0f, 1f)).First();
        source.clip = clip;
        source.Play();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        VolumeControl.RemoveSE(source);
    }
}