using UnityEngine;
using System.Collections;

public class ScrolledGameObject : MonoBehaviour
{
    public float scrollRatio = 0.004f;
    public float deltaX = 30;
    protected InfiniteScroll scroll;
    protected int delta;
    protected int localIndex;
    protected float initialX;

    // Use this for initialization
    protected virtual void Start()
    {
        localIndex = transform.GetSiblingIndex();
        scroll = FindObjectOfType<InfiniteScroll>();
        initialX = transform.localPosition.x;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        var x = scroll.xPosition * scrollRatio + initialX;
        var pos = transform.position;
        pos.x = x % deltaX;
        var index = localIndex - ((int) (x / deltaX) * 6);
        transform.localPosition = pos;
    }
}