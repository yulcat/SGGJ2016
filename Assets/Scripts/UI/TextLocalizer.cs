using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class TextLocalizer : MonoBehaviour
    {
        public string textKey;

        void OnEnable()
        {
            if (string.IsNullOrEmpty(textKey))
            {
                Debug.LogError($"Text Localizer Key is Missing in : {gameObject}");
                return;
            }
            var text = GetComponent<Text>();
            text.text = DB.MessageDB[textKey];
        }
    }
}