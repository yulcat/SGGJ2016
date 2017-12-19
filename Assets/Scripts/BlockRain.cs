using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class BlockRain : MonoBehaviour
{
    public GameObject[] blocks;
    public float radius = 10f;
    public float height = 30f;
    public float bps = 10f;
    List<GameObject> spawnedBlocks = new List<GameObject>();

    void SpawnBlock()
    {
        var block = blocks.OrderBy(s => Random.Range(0f, 1f)).First();
        var obj = Instantiate<GameObject>(block);
        var circle = Random.insideUnitCircle * radius;
        obj.transform.position = new Vector3(circle.x, height, circle.y);
        obj.transform.rotation = Random.rotation;
        var d = obj.GetComponent<Rigidbody>();
        d.angularDrag = 0.6f;
        d.constraints = RigidbodyConstraints.None;
        var b = obj.GetComponent<Block>();
        Destroy(b);
        spawnedBlocks.Add(obj);
    }

    IEnumerator Start()
    {
        while (true)
        {
            var idle = spawnedBlocks.FirstOrDefault(b => b.transform.position.y < -3);
            if (idle == null)
                SpawnBlock();
            else
            {
                var circle = Random.insideUnitCircle * radius;
                idle.transform.position = new Vector3(circle.x, height, circle.y);
                idle.transform.rotation = Random.rotation;
            }
            yield return new WaitForSeconds(1f / bps);
        }
    }
}