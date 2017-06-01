using UnityEngine;

public class UIWithSound : MonoBehaviour
{
    public void PlaySound(string clipName)
	{
		var audioSource = GetComponent<AudioSource>();
		if(audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
		audioSource.clip = UISoundResource.GetClip(clipName);
		audioSource.volume = VolumeControl.seVol;
		audioSource.Play();
	}
}