using UnityEngine;
using System.Collections;

public class SingletonBase <T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T _instance;
	public static T _Instance
	{
		get
		{
			if( FindObjectOfType(typeof(T)) == null )
			{
				GameObject obj = new GameObject();
				obj.name = typeof(T).ToString ();
				_instance = obj.AddComponent<T>();
				
				DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}
}