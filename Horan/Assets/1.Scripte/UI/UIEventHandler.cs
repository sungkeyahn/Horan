using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnBeginDragHandler = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke(eventData);
    }

    public Action<PointerEventData> OnDragHandler = null;
    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public Action<PointerEventData> OnPointerClickHandler = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnPointerClickHandler != null)
            OnPointerClickHandler.Invoke(eventData);
    }
}
