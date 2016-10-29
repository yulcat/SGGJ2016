using UnityEngine;
using System.Collections;

public class BalloonLine : MonoBehaviour {
	public Transform target;
	LineRenderer line;
	void Awake()
	{
		line = GetComponent<LineRenderer>();
	}
	// Update is called once per frame
	void Update () {
		line.SetPosition(1,transform.InverseTransformPoint(target.position + Vector3.up * 0.5f));
	}
}
