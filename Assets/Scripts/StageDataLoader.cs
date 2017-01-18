using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageDataLoader {
	public struct StageMiddleData
	{
		public int stageNumber;
		public string theme;
		public string mission;
	}
	public static StageMiddleData GetStageData(int stage)
	{
		if(loaded == null) LoadStageData();
		return loaded.Find(d => d.stageNumber == stage);
	}
	static List<StageMiddleData> loaded;

	static void LoadStageData() {
		loaded = CSVLoader.LoadList<StageMiddleData>("Data/stages");
	}
}
