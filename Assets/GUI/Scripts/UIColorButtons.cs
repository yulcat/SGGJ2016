using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIColorButtons : MonoBehaviour 
{
	public Button[] btns;
	public Color[] allColors;

	private void Awake()
	{
		allColors = new Color[btns.Length];
		for(int i=0; i<btns.Length; i++){
			allColors[i] = btns[i].GetComponent<Image>().color;
			btns[i].GetComponent<Image>().color = Color.white;
		}
	}

	private IEnumerator Start()
	{
		for(int i=0; i<btns.Length; i++){
			yield return new WaitForSeconds(0.1f);
			btns[i].GetComponent<LerpButton>().LerpColor(allColors[i]);
		}
	}
}
