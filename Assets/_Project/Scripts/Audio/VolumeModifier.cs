using System;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeModifier : IDisposable
{
    private const float MinimumLevel = -80;
    private const float MaximumLevel = 20;

    private readonly AudioMixer _mixer;
    private readonly SliderInformer _slider;
    private readonly string _group;

    private float _minimumValueSlider;
    private float _maximumValueSlider;

    public VolumeModifier(AudioMixer mixer, SliderInformer slider, string group)
    {
        _mixer = mixer != null ? mixer : throw new ArgumentNullException(nameof(mixer));
        _slider = slider != null ? slider : throw new ArgumentNullException(nameof(slider));
        _group = group;

        Init();
    }

    public void Dispose()
    {
        if (_slider != null)
            _slider.Changed -= OnChanged;
    }

    private void Init()
    {
        _minimumValueSlider = _slider.MinValue;
        _maximumValueSlider = _slider.MaxValue;

        SetLevel(_slider.Value);

        _slider.Changed += OnChanged;
    }

    private void SetLevel(float value)
    {
        float level = ConvertVolumeToLevel(NormalizeValue(value));
        _mixer.SetFloat(_group, level);
    }

    private float ConvertVolumeToLevel(float value) =>
        value == 0 ? MinimumLevel : Mathf.Log10(value) * MaximumLevel;

    private float NormalizeValue(float value) =>
        Mathf.InverseLerp(_minimumValueSlider, _maximumValueSlider, value);

    private void OnChanged(float value) =>
        SetLevel(value);
}