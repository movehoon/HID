using UnityEngine;
using System.Collections;
using LitJson;

public class JsonParser : SingletonBase<JsonParser>
{
	public Hashtable StringToHashTable(string data)
	{
		if (string.IsNullOrEmpty (data))
			return null;
		
		JsonReader reader = new JsonReader (data);
		
		while (reader.Read()) 
		{
			if(reader.Token == LitJson.JsonToken.ObjectStart)
			{
				return JsonParseLoop(reader);
			}
		}
		
		return null;
	}
	
	public string HashtableToJsonString(Hashtable hashTable)
	{
		return JsonMapper.ToJson (hashTable);
	}
	
	private Hashtable JsonParseLoop(JsonReader reader)
	{
		Hashtable hashTable = new Hashtable ();
		
		object key = null;
		object val = null;
		
		while (reader.Read())
		{
			if(reader.Token == LitJson.JsonToken.ObjectEnd)
			{
				return hashTable;
			}
			else if(reader.Token == LitJson.JsonToken.ObjectStart)
			{
				hashTable.Add(key, JsonParseLoop(reader));
			}
			else if(reader.Token == LitJson.JsonToken.PropertyName)
			{
				key = reader.Value; 
			}
			else
			{
				val = reader.Value;
			}
			
			if( key != null && val != null )
			{
				hashTable.Add (key, val);
				key = null;
				val = null;
			}
		}
		
		return hashTable;
	}
}