using UnityEngine;

namespace Nabla.UIOnly
{
    public class LoadSceneAfterSeconds : MonoBehaviour
    {
        public float SceneLoadTime = 12f;

        // Use this for initialization
        void Start()
        {
            Invoke(nameof(SceneLoad), SceneLoadTime);
        }

        void SceneLoad()
        {
            SceneLoader.LoadSceneByName("StageSelect");
        }
    }
}