using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ClickArea : UIBehaviour, IPointerClickHandler, ICanvasRaycastFilter {
	public UnityEvent OnClick;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
		Debug.Log(sp);
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
		Debug.Log("Click");
        OnClick.Invoke();
    }
}
