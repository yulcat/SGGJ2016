using UnityEngine;
using System.Collections;

public class ScrolledGameObject : MonoBehaviour {
	public float scrollRatio = 0.004f;
	public float deltaX = 30;
	public bool withButton = true;
	InfiniteScroll scroll;
	int delta;
	int localIndex;
	float initialX;
	StageIndexText button;

	// Use this for initialization
	void Start () {
		localIndex = transform.GetSiblingIndex();
		scroll = FindObjectOfType<InfiniteScroll>();
		initialX = transform.localPosition.x;
		if(withButton)
		{
			var buttonObj = Instantiate<GameObject>(Resources.Load<GameObject>("StageButton"));
			buttonObj.transform.SetParent(GameObject.Find("IndexCanvas").transform);
			// buttonObj.transform.localScale = Vector3.one;
			button = buttonObj.GetComponent<StageIndexText>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		float x = scroll.xPosition * scrollRatio + initialX;
		var pos = transform.position;
		pos.x = x % deltaX;
		var index = localIndex - ((int)(x / deltaX) * 6);
		if(button)
		{
			button.index = index;
			var rect = button.GetComponent<RectTransform>();
			// var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
			// screenPoint += Vector3.left * Screen.width * 0.5f;
			// screenPoint *= 1080f / Screen.height;
			pos.z = Mathf.Sin(index) * 0.5f;
			transform.localPosition = pos;
			button.transform.position = transform.position;
			GetComponent<LoadStageButton>().index = index;
		}
		else
		{
			transform.localPosition = pos;
		}
	}
}
