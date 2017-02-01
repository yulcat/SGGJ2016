using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

public class CSVLoader {
	public static Dictionary<string,T> LoadDictionary<T>(string path)
    {
        TextAsset data = Resources.Load(path) as TextAsset;
        return ToDictionary<T>(data.text);
    }

    public static List<T> LoadList<T>(string path)
    {
        TextAsset data = Resources.Load(path) as TextAsset;
        return ToList<T>(data.text);
    }

	public static T ToObject<T>(List<string> splitted)
	{
		Type type = typeof(T);
		if(type.IsPrimitive || type == typeof(string))
			return (T)Convert.ChangeType(splitted[0],type);
        FieldInfo[] fields = type.GetFields();
        object newT = Activator.CreateInstance(type);
        for(int i=0; i<fields.Length; i++)
        {
			if(splitted.Count() == i) break;
			if(splitted[i] == "") continue;
            fields[i].SetValue(newT,Convert.ChangeType(splitted[i],fields[i].FieldType));
        }
        return (T)newT;
	}

	public static Dictionary<string,T> ToDictionary<T>(string csv)
	{
		CSVDictionaryParser<T> parser = new CSVDictionaryParser<T>(csv);
		return parser.parsed;
	}

	public static List<T> ToList<T>(string csv)
	{
		CSVListParser<T> parser = new CSVListParser<T>(csv);
		return parser.parsed;
	}

	public static string ToCSV<T>(Dictionary<string,T> dic)
	{
		Type type = typeof(T);
		FieldInfo[] fields = type.GetFields();
		string csv = "name,";
		foreach(FieldInfo field in fields)
		{
			csv += field.Name;
			csv += ",";
		}
		csv += "\n";
		foreach(KeyValuePair<string,T> pair in dic)
		{
			csv = csv + pair.Key + ToLine(pair.Value) + "\n";
		}
		return csv;
	}
	public static string ToLine(object o)
	{
		Type type = o.GetType();
		FieldInfo[] fields = type.GetFields();
		string line = "";
		
		foreach(FieldInfo field in fields)
		{
			string parsed;
			if(field.GetValue(o) == null)
				parsed = "";
			else
				parsed = field.GetValue(o).ToString();
			if(parsed.Contains(",") || parsed.Contains("\n")) parsed = "\"" + parsed + "\"";
			line = line + "," + parsed;
		}
		return line;
	}
	struct CSVDictionaryParser<T>
	{
		private Dictionary<string,T> dic;
		public Dictionary<string,T> parsed
		{
			get
			{
				return dic;
			}
		}
		public CSVDictionaryParser(string csv)
		{
			dic = new Dictionary<string,T>();
			fgCSVReader.LoadFromString(csv,AddKeyValuePair);
		}
		void AddKeyValuePair(int index, List<string> line)
		{
			if(index==0) return;
			string name = line[0];
			if(name == "") return;
			line.RemoveAt(0);
			dic.Add(name,CSVLoader.ToObject<T>(line));
		}
	}
	struct CSVListParser<T>
	{
		private List<T> list;
		public List<T> parsed
		{
			get
			{
				return list;
			}
		}
		public CSVListParser(string csv)
		{
			list = new List<T>();
			fgCSVReader.LoadFromString(csv,AddElement);
		}
		void AddElement(int index, List<string> line)
		{
			if(index == 0 || line[0]=="") return;
			list.Add(CSVLoader.ToObject<T>(line));
		}
	}
}