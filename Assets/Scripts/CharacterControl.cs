using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {
	Pyramid pyramid;
	XY moveTarget;
	XY position;
	public void SetPyramid (Pyramid p) {
		pyramid = p;
	}

	Plane GetPyramidPlane()
	{
		return new Plane(pyramid.transform.forward, pyramid.transform.position);
	}

	Vector3 GetClickPosition()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var plane = GetPyramidPlane();
		float dist;
		plane.Raycast(ray, out dist);
		return pyramid.transform.InverseTransformPoint(ray.GetPoint(dist));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			var pos = GetClickPosition();
			moveTarget = new XY(pos);
		}
	}
}
