using UnityEngine;

public class FrameAdjust : MonoBehaviour
{
    int prevFrameRate;

    void OnEnable()
    {
        prevFrameRate = Application.targetFrameRate;
        Application.targetFrameRate = 60;
    }

    void OnDisable()
    {
        Application.targetFrameRate = prevFrameRate;
    }
}