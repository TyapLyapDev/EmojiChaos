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
    [SerializeField] private TestInUpResetButton _testInUpResetButton;

    private Saver _saver;
    private VolumeModifier _musicModifier;
    private VolumeModifier _sfxModifier;
    private SceneLoader _sceneLoader;

    private void OnDestroy()
    {
        _musicModifier?.Dispose();
        _sfxModifier?.Dispose();

        if (_closerButton != null)
            _closerButton.Clicked -= OnCloseClicked;

        if (_testInUpResetButton != null)
            _testInUpResetButton.Clicked -= OnTestInUpResetClicked;
    }

    public void Initialize(Saver saver, SceneLoader sceneLoader)
    {
        Initialize();

        _saver = saver ?? throw new ArgumentNullException(nameof(saver));
        _sceneLoader = sceneLoader != null ? sceneLoader : throw new ArgumentNullException(nameof(sceneLoader));

        InitializeVolumeSliders();
        InitializeVolumeModifiers();
        InitializeVolumeButtons();

        _languageSwitcher.Initialize();
        _testInUpResetButton.Initialize();

        _closerButton.Clicked += OnCloseClicked;
        _testInUpResetButton.Clicked += OnTestInUpResetClicked;
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

    private void OnTestInUpResetClicked(TestInUpResetButton button)
    {
        _saver.ResetInUp();
        _saver.Save();
        _sceneLoader.LoadScene(Constants.MenuSceneName);
    }
}