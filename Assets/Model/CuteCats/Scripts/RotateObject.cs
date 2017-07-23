using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0) ){
			this.transform.Rotate( Vector3.up *-15* Input.GetAxis("Mouse X") );
		}
		if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

	}

}
