using UnityEngine;
using System.Collections;

public class SlowSand : MonoBehaviour {
	void OnTriggerStay (Collider col) {
		col.GetComponent<Collider>().GetComponent<Rigidbody>().velocity *= 0.5f;
	}
}
