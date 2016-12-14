using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageData : MonoBehaviour {
	public struct MessageBox
	{
		public string message;
	}
	static Dictionary<string, MessageBox> _dictionary;
	public static Dictionary<string, MessageBox> dictionary
	{
		get
		{
			if(_dictionary == null)
			{
				var text = Resources.Load<TextAsset>("Data/messages");
				_dictionary = CSVLoader.ToDictionary<MessageBox>(text.text);
			}
			return _dictionary;
		}
	}
}
