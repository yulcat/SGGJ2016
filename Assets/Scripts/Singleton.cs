using UnityEngine;
using System.Collections;

public class Singleton<T> where T : MonoBehaviour {
	T _instance;
	public T instance
	{
		get
		{
			if(_instance == null)
			{
				var obj = new GameObject();
				_instance = obj.AddComponent<T>();
			}
			return _instance;
		}
	}
}
