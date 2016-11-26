using UnityEngine;
using System.Collections;

public class SlowSand : MonoBehaviour {
	public float damp = 0.5f;
	void OnTriggerStay (Collider col) {
		col.GetComponent<Collider>().GetComponent<Rigidbody>().velocity *= 1-damp;
	}
}
