using UnityEngine;
using System.Collections;

public class ScrolledGameObject : MonoBehaviour {
	public float scrollRatio = 0.004f;
	public float deltaX = 30;
	InfiniteScroll scroll;
	int delta;

	// Use this for initialization
	void Start () {
		scroll = FindObjectOfType<InfiniteScroll>();
	}
	
	// Update is called once per frame
	void Update () {
		float x = scroll.xPosition * scrollRatio;
		var pos = transform.position;
		pos.x = x % deltaX;
		transform.position = pos;
	}
}
