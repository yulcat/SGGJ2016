using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public static class StageDataLoader {
	public struct StageMiddleData
	{
		public int stageNumber;
		public string theme;
		public string mission;
	}
	public struct StageData
	{
		public int stageNumber;
		public StageManager.Theme theme;
		public Dictionary<string, int> mission;
		public StageData(StageMiddleData input)
		{
			stageNumber = input.stageNumber;
			theme = (StageManager.Theme)System.Enum.Parse(typeof(StageManager.Theme),input.theme);
			if(string.IsNullOrEmpty(input.mission))
				mission = null;
			else
				mission = JsonMapper.ToObject<Dictionary<string,int>>(input.mission);
		}
	}
	static IEnumerable<StageData> _stageData;
	public static StageData GetStageData(int stage)
	{
		if(_stageData == null)
		{
			var loaded = CSVLoader.LoadList<StageMiddleData>("Data/stages");
			_stageData = loaded.Select(d => new StageData(d));
		}
		return _stageData.First(d => d.stageNumber == stage);
	}
}
