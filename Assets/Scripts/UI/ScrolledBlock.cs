using UnityEngine;

public class ScrolledBlock : ScrolledGameObject {
	StageIndexText button;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		var buttonObj = Instantiate<GameObject>(Resources.Load<GameObject>("StageButton"));
		buttonObj.transform.SetParent(GameObject.Find("IndexCanvas").transform);
		// buttonObj.transform.localScale = Vector3.one;
		button = buttonObj.GetComponent<StageIndexText>();
	}
	
	// Update is called once per frame
	protected override void Update () {
		float x = scroll.xPosition * scrollRatio + initialX;
		var pos = transform.position;
		pos.x = x % deltaX;
		var index = localIndex - ((int)(x / deltaX) * 6);
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
}
