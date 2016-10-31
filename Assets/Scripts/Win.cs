using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : VRListener
{
    bool active;
    void Start()
    {
        Invoke("Activate", 1f);
    }
    void Activate()
    {
        active = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        if (Input.GetMouseButtonDown(0))
        {
            LoadScene();
        }
    }

    public override void OnClick()
    {
        LoadScene();
    }

    void LoadScene()
    {
        var current = SceneManager.GetActiveScene().buildIndex;
        if (current == SceneManager.sceneCountInBuildSettings - 1)
            SceneLoader.LoadScene(0);
        else
            SceneLoader.LoadScene(current + 1);
    }
}
