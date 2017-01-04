using UnityEngine;
using UnityEditor;
using System.Linq;

public class CameraSetter : MonoBehaviour {
	public int startingNumber = 3;
	public int endingNumber = 9;
	Camera[] cameras;
	public bool run = false;
	void OnDrawGizmos()
	{
		if(!run) return;
		cameras = GetComponentsInChildren<Camera>();
		if(cameras.Length != endingNumber - startingNumber + 1)
			Debug.LogError("Camera count != camera numbers");
		var initial = cameras[0].transform;
		var ending = cameras[cameras.Length-1].transform;
		for(int i=0; i<cameras.Length; i++)
		{
			var rate = (float)i/(cameras.Length-1);
			cameras[i].transform.position = Vector3.Lerp(initial.position, ending.position, rate);
			cameras[i].transform.rotation = Quaternion.Lerp(initial.rotation, ending.rotation, rate);
		}
		foreach(var c in cameras)
		{
			c.depth = -1;
		}
		if(cameras.Any(c => c.gameObject == Selection.activeGameObject))
		{
			cameras.First(c => c.gameObject == Selection.activeGameObject).depth = 0;
		}
	}

	void Start()
	{
		cameras = GetComponentsInChildren<Camera>();
		gameObject.SetActive(false);
	}

	public Transform GetProperTransform(int height)
	{
		return cameras[height+startingNumber].transform;
	}
}
