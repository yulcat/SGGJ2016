using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindowPop : Window {
	public Text text;
	public RectTransform window;
	public string Message
	{
		get
		{
			return text.text;
		}
		set
		{
			text.text = value;
		}
	}
	static WindowPop instanciated;
	public static void Open(string message)
    {
        var canvas = FindObjectOfType<Canvas>();
        if (instanciated != null)
        {
            instanciated.OpenWindow();
            return;
        }
        var newWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_Pop"));
        newWindow.transform.SetParent(canvas.transform, false);
        instanciated = newWindow.GetComponent<WindowPop>();
        instanciated.OpenWindow();
		instanciated.Message = message;
    }
	protected override void OnEnable()
	{
		window.localScale = Vector3.one;
		window.DOScale(Vector3.one * 0.5f, timeToOpenWindow).From().SetEase(Ease.OutBack);
	}
	public override void BackToPrevWindow()
	{
		window.DOScale(Vector2.zero, 0.3f)
			.SetEase(Ease.InBack)
			.OnComplete(() => WindowManager.instance.BackToPrevWindow());
	}
}
