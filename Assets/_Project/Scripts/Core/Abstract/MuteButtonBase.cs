using UnityEngine;

public abstract class MuteButtonBase<T> : ButtonClickHandler<T> where T : MuteButtonBase<T>
{
    [SerializeField] protected GameObject _disabledIcon;
    [SerializeField] protected SliderInformer _sliderInformer;

    protected float _savedVolume;

    public bool IsZero => Mathf.Approximately(_sliderInformer.Value, 0);

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_sliderInformer != null)
            _sliderInformer.Changed -= OnSliderChanged;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _sliderInformer.Changed += OnSliderChanged;

        _savedVolume = Mathf.Approximately(_sliderInformer.Value, 0) ? 0.5f : _sliderInformer.Value;
        OnSliderChanged(_sliderInformer.Value);
    }

    protected override void OnClick()
    {
        if (IsZero)
        {
            _sliderInformer.SetValue(_savedVolume);
        }
        else
        {
            _savedVolume = _sliderInformer.Value;
            _sliderInformer.SetValue(0);
        }

        base.OnClick();
    }

    protected virtual void OnSliderChanged(float value) =>
        _disabledIcon.SetActive(IsZero);
}