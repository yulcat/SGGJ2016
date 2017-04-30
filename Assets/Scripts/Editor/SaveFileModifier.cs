using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SaveFileModifier : Editor {
	[MenuItem("Tools/Edit Save File/Clear All Stages")]
	static void ClearAll () {
		var max = StageDataLoader.GetMaxStage();
		for(int i=0; i<max; i++)
		{
			if(SaveDataManager.clearRecord.ContainsKey(i.ToString()))
				continue;
			var clear = new SaveDataManager.ClearData();
			SaveDataManager.clearRecord.Add(i.ToString(),clear);
		}
		SaveDataManager.Save();
	}
	
	[MenuItem("Tools/Edit Save File/Delete Save Data")]
	static void Delete () {
		SaveDataManager.clearRecord.Clear();
		SaveDataManager.Save();
	}

	[MenuItem("Tools/Edit Hearts/Zero Heart")]
	static void ZeroHeart () {
		var hearts = SaveDataManager.data.heartLeft;
		for(int i=0; i<hearts; i++)
		{
			HeartManager.SpendHeart();
		}
	}
}
