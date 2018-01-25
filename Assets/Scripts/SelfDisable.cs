using UnityEngine;
using System.Collections;

public class SelfDisable : MonoBehaviour
{
    public float disableTime = 1f;

    // Use this for initialization
    void OnEnable()
    {
        Invoke("DisableSelf", disableTime);
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}