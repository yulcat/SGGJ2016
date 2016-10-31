using UnityEngine;
using System.Collections;
using Valve.VR;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VRControl : MonoBehaviour {
	public Transform shootPosition;
	public float shootSpeed = 10f;
	SteamVR_TrackedObject trackedObj;
	CharacterControl character;
	GameObject bulletOriginal;
	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		character = FindObjectOfType<CharacterControl>();
		bulletOriginal = Resources.Load<GameObject>("Bullet");
	}
	
	// Update is called once per frame
	void Update () {
		var device = SteamVR_Controller.Input((int)trackedObj.index);
		if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			// var bullet = Instantiate<GameObject>(bulletOriginal);
			var bullet = EffectSpawner.GetEffect("Bullet");
			bullet.transform.position = shootPosition.transform.position;
			bullet.GetComponent<Rigidbody>().velocity = -shootPosition.up * shootSpeed;
			bullet.GetComponent<Bullet>().Invoke("DisableSelf",15f);
		}
		var pressed = device.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
		var axis = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
		if(pressed)
		{
			character.OverrideInput(axis);
		}
	}
}
