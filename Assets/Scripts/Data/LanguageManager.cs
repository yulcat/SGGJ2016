using UnityEngine;

namespace Data
{
    public class LanguageManager
    {
        public enum Language
        {
            Korean,
            English
        }

        public static Language GetLang()
            => Application.systemLanguage == SystemLanguage.Korean ? Language.Korean : Language.English;

//        public static string GetString(string key)
//            => DB.MessageDB[key];
    }
}