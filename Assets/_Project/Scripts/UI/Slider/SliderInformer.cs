using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderInformer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Slider _slider;

    public event Action<float> Changed;
    public event Action PointerDownPressed;
    public event Action PointerUpPressed;

    public float Value => _slider.value;

    public float MinValue => _slider.minValue;

    public float MaxValue => _slider.maxValue;

    private void OnEnable() =>
        _slider.onValueChanged.AddListener(OnChanged);

    private void OnDisable() =>
        _slider.onValueChanged.RemoveListener(OnChanged);

    public void OnPointerDown(PointerEventData _)
    {
        Audio.Sfx.PlayPointerDownButton();
        PointerDownPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData _)
    {
        Audio.Sfx.PlayPointerUpButton();
        PointerUpPressed?.Invoke();
    }

    public void SetValue(float value) =>
        _slider.value = value;

    private void OnChanged(float value) =>
        Changed?.Invoke(value);
}