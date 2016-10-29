using UnityEngine;
using System.Collections;
using System.Linq;

public class BlockRain : MonoBehaviour {
	public GameObject[] blocks;
	public float radius = 10f;
	public float height = 30f;
	public float bps = 10f;
	void SpawnBlock()
	{
		var block = blocks.OrderBy(s => Random.Range(0f,1f)).First();
		var obj = Instantiate<GameObject>(block);
		var circle = Random.insideUnitCircle * radius;
		obj.transform.position = new Vector3(circle.x, height, circle.y);
		obj.transform.rotation = Random.rotation;
		var d = obj.GetComponent<Rigidbody>();
		d.angularDrag = 0.6f;
		var b = obj.GetComponent<Block>();
		b.lifeTime = 10f;
		b.SetPyramid(null);
		b.FallOff();
	}
	
	IEnumerator Start () {
		while(true)
		{
			SpawnBlock();
			yield return new WaitForSeconds(1f/bps);
		}
	}
}
