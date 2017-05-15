using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindowYNPop : WindowPop {
	static WindowYNPop instanciated;
	System.Action onSelect;
	System.Action onCancel;
	public static void OpenYNPop(string message, System.Action select, System.Action cancel = null)
    {
        var canvas = FindObjectOfType<Canvas>();
        if (instanciated != null)
        {
            instanciated.OpenWindow();
            return;
        }
        var newWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_YNPop"));
        newWindow.transform.SetParent(canvas.transform, false);
        instanciated = newWindow.GetComponent<WindowYNPop>();
        instanciated.OpenWindow();
		instanciated.Message = message;
		instanciated.onSelect = select;
		instanciated.onCancel = cancel;
    }
	public void Select()
	{
		base.BackToPrevWindow();
		if(onSelect != null) onSelect();
	}
	public override void BackToPrevWindow()
	{
		base.BackToPrevWindow();
		if(onCancel != null) onCancel();
	}
	protected override void OnEnable()
	{
		window.localScale = Vector3.one;
		window.DOScale(Vector3.one * 0.5f, timeToOpenWindow).From().SetEase(Ease.OutBack);
	}
	public override void CloseAllWindow()
	{
		window.DOScale(Vector2.zero, 0.3f)
			.SetEase(Ease.InBack)
			.OnComplete(() => WindowManager.instance.CloseAllWindow());
	}
}
