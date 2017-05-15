using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Window : MonoBehaviour {
	protected const float timeToOpenWindow = 0.3f;
	public void OpenWindow()
	{
		WindowManager.instance.OpenWindow(this);
	}
	public virtual void CloseAllWindow()
	{
		var rect = GetComponent<RectTransform>();
		rect.DOScale(Vector2.zero, 0.3f)
			.SetEase(Ease.InBack)
			.OnComplete(() => WindowManager.instance.CloseAllWindow());
	}
	public virtual void BackToPrevWindow()
	{
		WindowManager.instance.BackToPrevWindow();
	}
	protected virtual void OnEnable()
	{
		var rect = GetComponent<RectTransform>();
		rect.localScale = Vector3.one;
		rect.DOScale(Vector3.one * 0.5f, timeToOpenWindow).From().SetEase(Ease.OutBack).SetUpdate(true);
	}
}
