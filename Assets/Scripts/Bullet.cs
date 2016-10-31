using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	bool active = false;
	void OnEnable()
	{
		active = true;
		CancelInvoke();
	}
	void OnCollisionEnter(Collision col)
	{
		var other = col.collider.GetComponent<Block>();
		if(!active || other==null) return;
		active = false;
		other.SendMessage("OnMouseDown");
	}
	void DisableSelf()
	{
		gameObject.SetActive(false);
	}
}
