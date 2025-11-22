using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonClickHandler<T> : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler 
    where T : ButtonClickHandler<T> 
{
    public event Action<T> Clicked;
    public event Action<T> Pressed;
    public event Action<T> Unpressed;

    protected T Self => (T)this;

    public void OnPointerClick(PointerEventData eventData) =>
        Clicked?.Invoke(Self);

    public void OnPointerDown(PointerEventData eventData) =>
        Pressed?.Invoke(Self);

    public void OnPointerUp(PointerEventData eventData) =>
        Unpressed?.Invoke(Self);
}