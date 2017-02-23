using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockIconTextureApplier : MonoBehaviour {
	[System.SerializableAttribute]
	public struct BlockIconPreset
	{
		public StageManager.Theme theme;
		public BlockType type;
		public Texture texture;
		public Texture uvTexture;
	}
	public BlockIconPreset[] blockIconPresetList;
	public int toLoad;
	private Image img;
	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		img = GetComponent<Image>();
	}
	public void LoadIcon()
	{
		var preset = blockIconPresetList[toLoad];
		img = GetComponent<Image>();
		var material = new Material(img.material);
		material.SetTexture("_MainTex",preset.texture);
		material.SetTexture("_UVTex",preset.uvTexture);
		img.material = material;
	}
}
