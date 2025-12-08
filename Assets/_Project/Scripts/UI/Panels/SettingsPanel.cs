using System;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanel : PanelBase
{
    [SerializeField] private SettingsPanelCloserButton _closerButton;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private SliderInformer _music;
    [SerializeField] private SliderInformer _sfx;
    [SerializeField] private SfxMuteButton _sfxMuteButton;
    [SerializeField] private MusicMuteButton _musicMuteButton;
    [SerializeField] private LanguageSwitcher _languageSwitcher;

    private Saver _saver;
    private VolumeModifier _musicModifier;
    private VolumeModifier _sfxModifier;

    private void OnDestroy()
    {
        _musicModifier?.Dispose();
        _sfxModifier?.Dispose();

        if (_closerButton != null)
            _closerButton.Clicked -= OnCloseClicked;
    }

    public void Initialize(Saver saver)
    {
        Initialize();

        _saver = saver ?? throw new ArgumentNullException(nameof(saver));

        InitializeVolumeSliders();
        InitializeVolumeModifiers();
        InitializeVolumeButtons();

        _languageSwitcher.Initialize();

        _closerButton.Clicked += OnCloseClicked;
    }

    public void ResetProgress()
    {
        _saver.ResetProgress();
        _saver.Save();

        ResetSlidersToSavedValues();
    }

    private void InitializeVolumeSliders() =>
        ResetSlidersToSavedValues();

    private void InitializeVolumeModifiers()
    {
        _musicModifier = new(_mixer, _music, Constants.MusicGroup);
        _sfxModifier = new(_mixer, _sfx, Constants.SfxGroup);
    }

    private void InitializeVolumeButtons()
    {
        _sfxMuteButton.Initialize();
        _musicMuteButton.Initialize();
    }

    private void SaveVolumeSettings()
    {
        _saver.SetMusicVolume(_music.Value);
        _saver.SetSfxVolume(_sfx.Value);
        _saver.Save();
    }

    private void ResetSlidersToSavedValues()
    {
        _music.SetValue(_saver.MusicVolume);
        _sfx.SetValue(_saver.SfxVolume);
    }

    private void OnCloseClicked(SettingsPanelCloserButton _) =>
        SaveVolumeSettings();
}