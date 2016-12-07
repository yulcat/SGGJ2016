using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIToggleBtnExample : MonoBehaviour
{
	public bool state = false;
	public RectTransform handle;
	public RectTransform handleColor;
	public Color onColor;
	public Color offColor;

	private void Start()
	{
		OnClick();
	}				

	public void OnClick()
	{
		state = !state;		
		if(state){
			handle.anchoredPosition = new Vector2(13, handle.anchoredPosition.y);
			handleColor.GetComponent<Image>().color = onColor;
		}else{
			handle.anchoredPosition = new Vector2(-13, handle.anchoredPosition.y);
			handleColor.GetComponent<Image>().color = offColor;
		}	
	}
	
}
