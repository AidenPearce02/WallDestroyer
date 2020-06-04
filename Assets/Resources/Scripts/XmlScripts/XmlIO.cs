using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class XmlIO
{
	public static void SaveXml<T> (this object deserializedXml, string path) where T : class
	{
		using(var stream = new FileStream(path, FileMode.Create))
		{
			var s = new XmlSerializer(typeof(T));
			s.Serialize(stream, deserializedXml);
		}
	}

	public static T LoadXml<T>(string textAssetName) where T : class
	{
		TextAsset xmlTextAsset = (TextAsset) Resources.Load (textAssetName, typeof(TextAsset));

		using(var stream = new StringReader(xmlTextAsset.text))
		{
			var s = new XmlSerializer(typeof(T));			 
			return s.Deserialize(stream) as T;
		}
	}

	public static List<T> LoadAllXmlLevels<T>() where T : class {
		List<T> list=new List<T>();
		var xmlTextAssets = Resources.LoadAll("Levels/", typeof(TextAsset)).Cast<TextAsset>();
		foreach(TextAsset level in xmlTextAssets)
		{
			using (var stream = new StringReader(level.text))
			{
				var s = new XmlSerializer(typeof(T));
				list.Add(s.Deserialize(stream) as T);
			}
		}
		return list;
	}
}
